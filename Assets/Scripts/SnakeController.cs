using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Alive,
    Dead
}

public class SnakeController : MonoBehaviour
{
    private enum Direction
    {
        Left,
        Right,
        Up,
        Down
    } 

    public State state;
    private Direction gridMoveDirection;
    private Vector2Int gridPosition;

    private float snakeMoveTimer;
    [HideInInspector] public float snakeMoveTimerMax;

    private int snakeBodySize;
    private int width;
    private int height;

    private List<SnakeMovePosition> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;

    [HideInInspector] public bool deadFood = false;
    [HideInInspector] public bool shield = false;

    [SerializeField] private FoodSpawner foodSpawner;
    [SerializeField] private PowerUpController powerUp;
    [SerializeField] private GameOverWindow gameOver;

    private void Awake()
    {
        width = 20;
        height = 20;

        gridPosition = new Vector2Int(10, 10);
        snakeMoveTimerMax = 0.1f;
        snakeMoveTimer = snakeMoveTimerMax;
        gridMoveDirection = Direction.Right;

        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodySize = 0;

        snakeBodyPartList = new List<SnakeBodyPart>();

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
                break;
            case State.Dead:
                break;
        }
    }

    private void HandleInput()
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
    }

    private void HandleGridMovement()
    {
        snakeMoveTimer += Time.deltaTime;
        if (snakeMoveTimer >= snakeMoveTimerMax)
        {
            snakeMoveTimer -= snakeMoveTimerMax;

            SoundManager.Instance.Play(Sounds.SnakeMove);
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

            gridPosition += gridMoveDirectionVector;

            gridPosition = ValidateGridPosition(gridPosition);

            bool snakeAteFood = foodSpawner.EatFood(gridPosition);
            if (snakeAteFood)
            {
                // Snake ate food, grow body
                if (deadFood)
                {
                    if (snakeBodySize > 0)
                    {
                        RemoveSnakeBody();
                        snakeBodySize--;
                        SoundManager.Instance.Play(Sounds.SnakeEat);
                    }
                }
                else
                {
                    snakeBodySize++;
                    CreateSnakeBody();
                    SoundManager.Instance.Play(Sounds.SnakeEat);
                }                
            }
            bool snakePowerUp = powerUp.SnakePowerUp(gridPosition);

            if (snakeMovePositionList.Count >= snakeBodySize + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList)
            {
                Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
                if (gridPosition == snakeBodyPartGridPosition)
                {
                    if(!shield)
                    {
                        // Game Over!
                        Debug.Log("Snake Dead");
                        state = State.Dead;
                        SoundManager.Instance.Play(Sounds.SnakeDie);
                        gameOver.GameOver();
                    }
                }
            }

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) - 90);

            UpdateSnakeBody();
        }
    }

    private void CreateSnakeBody()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }

    private void RemoveSnakeBody()
    {
        Destroy(snakeBodyPartList[snakeBodyPartList.Count - 1].snakeBodyGameObject);
        snakeBodyPartList.RemoveAt(snakeBodyPartList.Count - 1);
    }

    private void UpdateSnakeBody()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
        }
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

    /*
     * Handles a Single Snake Body Part
     * */
    private class SnakeBodyPart
    {

        private SnakeMovePosition snakeMovePosition;
        private Transform transform;
        public GameObject snakeBodyGameObject;

        public SnakeBodyPart(int bodyIndex)
        {
            snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = AllSprites.Instance.SnakeBody1;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -1 - bodyIndex;
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
                case Direction.Up: // Currently going Up
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = 0;
                            break;
                        case Direction.Left: // Previously was going Left
                            angle = 0 + 45;
                            transform.position += new Vector3(.2f, .2f);
                            break;
                        case Direction.Right: // Previously was going Right
                            angle = 0 - 45;
                            transform.position += new Vector3(-.2f, .2f);
                            break;
                    }
                    break;
                case Direction.Down: // Currently going Down
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = 180;
                            break;
                        case Direction.Left: // Previously was going Left
                            angle = 180 - 45;
                            transform.position += new Vector3(.2f, -.2f);
                            break;
                        case Direction.Right: // Previously was going Right
                            angle = 180 + 45;
                            transform.position += new Vector3(-.2f, -.2f);
                            break;
                    }
                    break;
                case Direction.Left: // Currently going to the Left
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = +90;
                            break;
                        case Direction.Down: // Previously was going Down
                            angle = 180 - 45;
                            transform.position += new Vector3(-.2f, .2f);
                            break;
                        case Direction.Up: // Previously was going Up
                            angle = 45;
                            transform.position += new Vector3(-.2f, -.2f);
                            break;
                    }
                    break;
                case Direction.Right: // Currently going to the Right
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = -90;
                            break;
                        case Direction.Down: // Previously was going Down
                            angle = 180 + 45;
                            transform.position += new Vector3(.2f, .2f);
                            break;
                        case Direction.Up: // Previously was going Up
                            angle = -45;
                            transform.position += new Vector3(.2f, -.2f);
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
