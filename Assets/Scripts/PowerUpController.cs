using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
   public enum PowerUp
   {
        Shield,
        ScoreBoost,
        SpeedUp
   }

    public PowerUp PowerType;
    private Vector2Int powerPosition;
    [SerializeField] private SnakeController snake;
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
        while (snake.GetFullSnakeGridPositionList().IndexOf(powerPosition) != -1);

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

    public bool SnakePowerUp(Vector2Int snakeGridPosition)
    {
        if (snakeGridPosition == powerPosition)
        {
            switch (PowerType)
            {
                default:
                    break;
                case PowerUp.Shield:
                    snake.shield = true;
                    StartCoroutine(Shield());
                    break;
                case PowerUp.ScoreBoost:
                    GameHandler.scoreBoost = true;
                    StartCoroutine(BoostScore());
                    break;
                case PowerUp.SpeedUp:
                    snake.snakeMoveTimerMax = 0.07f;
                    StartCoroutine(SpeedUp());
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
            yield return new WaitForSeconds(6);
        }
    }

    public IEnumerator BoostScore()
    {
        yield return new WaitForSeconds(6);
        GameHandler.scoreBoost = false;
        yield break;
    }

    public IEnumerator SpeedUp()
    {
        yield return new WaitForSeconds(6);
        snake.snakeMoveTimerMax = 0.1f;
        yield break;
    }

    public IEnumerator Shield()
    {
        yield return new WaitForSeconds(6);
        snake.shield = false;
        yield break;
    }
}