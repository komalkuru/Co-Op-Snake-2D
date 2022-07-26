using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleFoodController : MonoBehaviour
{
    public enum Food
    {
        food,
        burnFood,
    }

    [SerializeField] private MultipleSnakeController snake1;
    [SerializeField] private MultipleSnakeController snake2;

    public Food foodType;
    public Food getFood = Food.burnFood;
    private Vector2Int foodPosition;

    public Player playerFoodId;

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
        }
        while (snake1.GetFullSnakeGridPositionList().IndexOf(foodPosition) != -1 && snake2.GetFullSnakeGridPositionList().IndexOf(foodPosition) != -1);

        if (snake1.GetSnakeFullSize() > 1)
        {
            int select = Random.Range(0, 3);
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
                    foodType = Food.burnFood;
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

    public bool EatFood(Vector2Int snakeGridPosition, Player PlayerType)
    {
        playerFoodId = PlayerType;

        if (snakeGridPosition == foodPosition)
        {
            switch (foodType)
            {
                default:
                    MultipleGameHandler.AddScore(playerFoodId);
                    switch (playerFoodId)
                    {
                        case Player.Player1:
                            snake1.deadFood = false;
                            break;
                        case Player.Player2:
                            snake2.deadFood = false;
                            break;
                    }
                    break;

                case Food.food:
                    MultipleGameHandler.AddScore(playerFoodId);
                    switch (playerFoodId)
                    {
                        case Player.Player1:
                            snake1.deadFood = false;
                            break;
                        case Player.Player2:
                            snake2.deadFood = false;
                            break;
                    }
                    break;

                case Food.burnFood:
                    MultipleGameHandler.SubtractScore(playerFoodId);
                    switch (playerFoodId)
                    {
                        case Player.Player1:
                            snake1.deadFood = true;
                            break;
                        case Player.Player2:
                            snake2.deadFood = true;
                            break;
                    }
                    //snake.deadFood = true;
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
            yield return new WaitForSeconds(4f);
        }
    }
}
