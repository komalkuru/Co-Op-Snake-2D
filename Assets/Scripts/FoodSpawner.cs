using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public enum Food
    {
        food,
        deadFood
    }

    public Food foodType;
    public Food nonEatableFood = Food.deadFood;
    private Vector2Int foodPosition;
    [SerializeField] private SnakeController snake;
    [SerializeField] private PowerUpController powerUp;
    private int width;
    private int height;

    private void Awake()
    {
        width = 20;
        height = 20;
        foodType = Food.food;
    }

    private void Start()
    {
        StartCoroutine(FoodTimer());
    }

    public void SpawnFood()
    {
        do
        {
            foodPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (snake.GetFullSnakeGridPositionList().IndexOf(foodPosition) != -1);

        if (snake.GetSnakeFullSize() > 1)
        {
            int select = Random.Range(0, 2);
            switch (select)
            {
                default:
                    GetComponent<SpriteRenderer>().sprite = AllSprites.Instance.EatbleFood;
                    foodType = Food.food;
                    break;
                case 0:
                    GetComponent<SpriteRenderer>().sprite = AllSprites.Instance.EatbleFood;
                    foodType = Food.food;
                    break;
                case 1:
                    GetComponent<SpriteRenderer>().sprite = AllSprites.Instance.BurnFood;
                    foodType = Food.deadFood;
                    break;
            }
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = AllSprites.Instance.EatbleFood;
            foodType = Food.food;
        }

        transform.position = new Vector3(foodPosition.x, foodPosition.y);
    }

    public bool EatFood(Vector2Int snakeGridPosition)
    {

        if (snakeGridPosition == foodPosition)
        {
            switch (foodType)
            {
                default:
                    GameHandler.AddScore();
                    snake.deadFood = false;
                    break;
                case Food.food:
                    snake.deadFood = false;
                    GameHandler.AddScore();
                    break;
                case Food.deadFood:
                    snake.deadFood = true;
                    GameHandler.SubtractScore();
                    
                    break;
            }
            SpawnFood();
            return true;
        }
        else return false;
    }

    public IEnumerator FoodTimer()
    {
        while (true)
        {
            SpawnFood();
            yield return new WaitForSeconds(6);
        }
    }
}
