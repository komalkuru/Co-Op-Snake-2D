using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Player
{
    Player1,
    Player2
}

public class MultipleSnakeController : MonoBehaviour
{
    private enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    private int width;
    private int height;

    public Player playerType;
    public State state;

    private Direction gridMoveDirection;
    private Vector2Int gridPosition;
    private Vector2Int previousGridPosition;
    private float gridMoveTimer;
    [HideInInspector] public float snakeMoveTimerMax;

    private int snakeBodySize;

    [HideInInspector] public bool deadFood = false;
    [HideInInspector] public bool shield = false;

    [SerializeField] private MultipleFoodController foodSpawner;
    [SerializeField] private MultiplePowerUpController powerUp;
    [SerializeField] private GameOverWindow gameOver;
    [SerializeField] private MultipleSnakeController anotherSnake;
    [SerializeField] private WinText winText;

    private List<SnakeMovePosition> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;

    private void Awake()
    {
        if(playerType == Player.Player1)
        {
            gridPosition = new Vector2Int(10, 5);
        }
        else
        {
            gridPosition = new Vector2Int(10, 15);
        }

        snakeMoveTimerMax = 0.1f;
        gridMoveTimer = snakeMoveTimerMax;
        gridMoveDirection = Direction.Right;

        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodySize = 0;

        snakeBodyPartList = new List<SnakeBodyPart>();

        width = 20;
        height = 20;

        state = State.Alive;
        gameOver.DisableUI();
    }

    public void ResetGame()
    {
        Awake();
    }

    private void Update()
    {
        switch (state)
        {
            case State.Alive:
                HandleInput();
                HandleGridMovement();
                PlayerDeathConditionCheck();
                break;

            case State.Dead:
                break;
        }
    }

    private void HandleInput()
    {
        if(playerType == Player.Player1)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (gridMoveDirection != Direction.Down)
                {
                    gridMoveDirection = Direction.Up;
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (gridMoveDirection != Direction.Up)
                {
                    gridMoveDirection = Direction.Down;
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (gridMoveDirection != Direction.Right)
                {
                    gridMoveDirection = Direction.Left;
                }
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (gridMoveDirection != Direction.Left)
                {
                    gridMoveDirection = Direction.Right;
                }
            }
        } else
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (gridMoveDirection != Direction.Down)
                {
                    gridMoveDirection = Direction.Up;
                }
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (gridMoveDirection != Direction.Up)
                {
                    gridMoveDirection = Direction.Down;
                }
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (gridMoveDirection != Direction.Right)
                {
                    gridMoveDirection = Direction.Left;
                }
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (gridMoveDirection != Direction.Left)
                {
                    gridMoveDirection = Direction.Right;
                }
            }
        }
    }

    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= snakeMoveTimerMax)
        {
            gridMoveTimer -= snakeMoveTimerMax;

            SnakeMovePosition previousSnakeMovePosition = null;
            if (snakeMovePositionList.Count > 0)
            {
                previousSnakeMovePosition = snakeMovePositionList[0];
            }

            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition, gridPosition, gridMoveDirection);
            snakeMovePositionList.Insert(0, snakeMovePosition);

            Vector2Int gridMoveDirectionVector;

            switch (gridMoveDirection)
            {
                default:
                case Direction.Right: gridMoveDirectionVector = new Vector2Int(+1, 0); break;
                case Direction.Left: gridMoveDirectionVector = new Vector2Int(-1, 0); break;
                case Direction.Up: gridMoveDirectionVector = new Vector2Int(0, +1); break;
                case Direction.Down: gridMoveDirectionVector = new Vector2Int(0, -1); break;
            }
            previousGridPosition = gridPosition;
            gridPosition += gridMoveDirectionVector;
            gridPosition = ValidateGridPosition(gridPosition);
            AteFoodCheck();

            if (snakeMovePositionList.Count >= snakeBodySize + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            if (gridPosition == anotherSnake.gridPosition)
            {
                SoundManager.Instance.Play(Sounds.SnakeDie);
                state = State.Dead;
                gameOver.GameOver();
            }

            foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList)
            {
                Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
                if (gridPosition == snakeBodyPartGridPosition)
                {
                    if (!shield)
                    {
                        SoundManager.Instance.Play(Sounds.SnakeDie);
                        state = State.Dead;
                        gameOver.GameOver();
                    }
                }
            }

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) - 90);

            UpdateSnakeBodyParts();
        }
    }

    private void AteFoodCheck()
    {
        bool snakeAteFood = foodSpawner.EatFood(gridPosition, playerType);
        if (snakeAteFood)
        {
            SoundManager.Instance.Play(Sounds.SnakeEat);
            if (deadFood)
            {
                if (snakeBodySize > 0)
                {
                    RemoveSnakeBody();
                    snakeBodySize--;
                }
            }
            else
            {
                snakeBodySize++;
                CreateSnakeBody();
            }
        }

        powerUp.SnakePowerUp(gridPosition, playerType);
    }

    public void PlayerDeathConditionCheck()
    {

        if (gridPosition == anotherSnake.gridPosition)
        {
            state = State.Dead;
            gameOver.GameOver();
            winText.SetWinText(0);
        }

        foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList)
        {
            Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
            if (gridPosition == snakeBodyPartGridPosition)
            {
                if (!shield)
                {
                    if (playerType == Player.Player1)
                    {
                        winText.SetWinText(2);
                    }
                    else
                    {
                        winText.SetWinText(1);
                    }
                    state = State.Dead;
                    gameOver.GameOver();
                }
            }
        }

        foreach (SnakeBodyPart snakeBodyPart in anotherSnake.snakeBodyPartList)
        {
            Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
            if (gridPosition == snakeBodyPartGridPosition)
            {
                if (!shield)
                {
                    if (playerType == Player.Player1)
                    {
                        winText.SetWinText(2);
                    }
                    else
                    {
                        winText.SetWinText(1);
                    }
                    state = State.Dead;
                    gameOver.GameOver();
                }
            }
        }
    }

    private void CreateSnakeBody()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(playerType));
    }

    private void UpdateSnakeBodyParts()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
        }
    }
    private void RemoveSnakeBody()
    {
        Destroy(snakeBodyPartList[snakeBodyPartList.Count - 1].snakeBodyGameObject);
        snakeBodyPartList.RemoveAt(snakeBodyPartList.Count - 1);
    }

    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    // Return the full list of positions occupied by the snake: Head + Body
    public List<Vector2Int> GetFullSnakeGridPositionList()
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
        foreach (SnakeMovePosition snakeMovePosition in snakeMovePositionList)
        {
            gridPositionList.Add(snakeMovePosition.GetGridPosition());
        }
        return gridPositionList;
    }

    private class SnakeBodyPart
    {

        private SnakeMovePosition snakeMovePosition;
        private Transform transform;
        public GameObject snakeBodyGameObject;

        public SnakeBodyPart(Player playerType)
        {
            snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            if (playerType == Player.Player2)
            {
                snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = AllSprites.Instance.SnakeBody2;
            }
            else
            {
                snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = AllSprites.Instance.SnakeBody1;
            }
            //snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.SnakeBodySprite;
            transform = snakeBodyGameObject.transform;
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
        {
            this.snakeMovePosition = snakeMovePosition;

            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);

            float angle;
            switch (snakeMovePosition.GetDirection())
            {
                default:
                case Direction.Up:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = 0;
                            break;
                        case Direction.Left:
                            angle = 0 + 45;
                            break;
                        case Direction.Right:
                            angle = 0 - 45;
                            break;
                    }
                    break;
                case Direction.Down:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = 180;
                            break;
                        case Direction.Left:
                            angle = 180 - 45;
                            break;
                        case Direction.Right:
                            angle = 180 + 45;
                            break;
                    }
                    break;
                case Direction.Left:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = +90;
                            break;
                        case Direction.Down:
                            angle = 180 - 45;
                            break;
                        case Direction.Up:
                            angle = 45;
                            break;
                    }
                    break;
                case Direction.Right:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = -90;
                            break;
                        case Direction.Down:
                            angle = 180 + 45;
                            break;
                        case Direction.Up:
                            angle = -45;
                            break;
                    }
                    break;
            }

            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        public Vector2Int GetGridPosition()
        {
            if (snakeMovePosition != null)
            {
                return snakeMovePosition.GetGridPosition();
            }

            else return new Vector2Int(0, 0);
        }
    }

    /*
     * Handles one Move Position from the Snake
     * */
    private class SnakeMovePosition
    {

        private SnakeMovePosition previousSnakeMovePosition;
        private Vector2Int gridPosition;
        private Direction direction;

        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2Int gridPosition, Direction direction)
        {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;
        }

        public Vector2Int GetGridPosition()
        {
            return gridPosition;
        }

        public Direction GetDirection()
        {
            return direction;
        }

        public Direction GetPreviousDirection()
        {
            if (previousSnakeMovePosition == null)
            {
                return Direction.Right;
            }
            else
            {
                return previousSnakeMovePosition.direction;
            }
        }
    }

    public Vector2Int ValidateGridPosition(Vector2Int gridPosition)
    {
        if (gridPosition.x < 0)
        {
            gridPosition.x = width - 1;
        }
        if (gridPosition.x > width - 1)
        {
            gridPosition.x = 0;
        }
        if (gridPosition.y < 0)
        {
            gridPosition.y = height - 1;
        }
        if (gridPosition.y > height - 1)
        {
            gridPosition.y = 0;
        }
        return gridPosition;
    }

    public int GetSnakeFullSize()
    {
        return snakeBodyPartList.Count;
    }
}
