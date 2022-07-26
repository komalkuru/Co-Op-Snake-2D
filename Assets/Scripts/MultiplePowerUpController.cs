using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplePowerUpController : MonoBehaviour
{
    public enum PowerUp
    {
        Shield,
        ScoreBoost,
        SpeedUp
    }

    [SerializeField] private MultipleSnakeController snake1;
    [SerializeField] private MultipleSnakeController snake2;

    public PowerUp PowerType;
    private Vector2Int powerPosition;

    public Player playerPowerId;

    private int width;
    private int height;

    private void Awake()
    {
        width = 20;
        height = 20;
        PowerType = PowerUp.Shield;
    }

    private void Start()
    {
        StartCoroutine(FoodTimer());
    }

    public void SpawnPowerUp()
    {
        do
        {
            powerPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        }
        while (snake1.GetFullSnakeGridPositionList().IndexOf(powerPosition) != -1 && snake2.GetFullSnakeGridPositionList().IndexOf(powerPosition) != -1);

        int select = Random.Range(0, 3);
        switch (select)
        {
            case 0:
                GetComponent<SpriteRenderer>().sprite = AllSprites.Instance.ScoreBoost;
                PowerType = PowerUp.ScoreBoost;
                break;
            case 1:
                GetComponent<SpriteRenderer>().sprite = AllSprites.Instance.Shield;
                PowerType = PowerUp.Shield;
                break;
            case 2:
                GetComponent<SpriteRenderer>().sprite = AllSprites.Instance.SpeedUp;
                PowerType = PowerUp.SpeedUp;
                break;
        }

        transform.position = new Vector3(powerPosition.x, powerPosition.y);
    }

    public bool SnakePowerUp(Vector2Int snakeGridPosition, Player Id)
    {
        playerPowerId = Id;

        if (snakeGridPosition == powerPosition)
        {
            SoundManager.Instance.Play(Sounds.SnakeEat);

            switch (PowerType)
            {
                default:
                    break;

                case PowerUp.Shield:
                    switch (playerPowerId)
                    {
                        case Player.Player1:
                            snake1.shield = true;
                            StartCoroutine(Shield(playerPowerId));
                            break;
                        case Player.Player2:
                            snake2.shield = true;
                            StartCoroutine(Shield(playerPowerId));
                            break;
                    }
                    break;

                case PowerUp.ScoreBoost:
                    MultipleGameHandler.scoreBoost = true;
                    StartCoroutine(BoostScore());
                    break;

                case PowerUp.SpeedUp:
                    switch (playerPowerId)
                    {
                        case Player.Player1:
                            snake1.snakeMoveTimerMax = 0.07f;
                            StartCoroutine(SpeedUp(playerPowerId));
                            break;
                        case Player.Player2:
                            snake2.snakeMoveTimerMax = 0.07f;
                            StartCoroutine(SpeedUp(playerPowerId));
                            break;
                    }
                    break;
            }

            SpawnPowerUp();
            return false;
        }
        else return false;
    }

    public IEnumerator FoodTimer()
    {
        while (true)
        {
            SpawnPowerUp();
            yield return new WaitForSeconds(Random.Range(4, 8));
        }
    }

    public IEnumerator BoostScore()
    {
        yield return new WaitForSeconds(3);
        MultipleGameHandler.scoreBoost = false;
        yield break;
    }

    public IEnumerator SpeedUp(Player playerType)
    {
        yield return new WaitForSeconds(3);

        if (playerType == Player.Player1)
        {
            snake1.snakeMoveTimerMax = 0.1f;
        }
        else if (playerType == Player.Player2)
        {
            snake2.snakeMoveTimerMax = 0.1f;
        }
        else
        {
            snake1.snakeMoveTimerMax = 0.1f;
            snake2.snakeMoveTimerMax = 0.1f;
        }

        yield break;
    }

    public IEnumerator Shield(Player Id)
    {
        yield return new WaitForSeconds(3);

        switch (Id)
        {
            default:
                snake1.shield = false;
                snake2.shield = false;
                break;
            case Player.Player1:
                snake1.shield = false;
                break;
            case Player.Player2:
                snake2.shield = false;
                break;
        }
        yield break;
    }
}
