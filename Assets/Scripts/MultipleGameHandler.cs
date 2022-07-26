using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleGameHandler : MonoBehaviour
{
    private static MultipleGameHandler instance;

    public static MultipleGameHandler Instance { get { return instance; } }

    private static int score1;
    private static int score2;
    public static bool scoreBoost;

    [SerializeField] private MultipleFoodController foodSpawn;
    [SerializeField] private MultipleSnakeController snake1;
    [SerializeField] private MultipleSnakeController snake2;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        InitializeStatic();
    }

    public static int GetScore(Player playerType)
    {
        if (playerType == Player.Player1)
        {
            return score2;
        }
        else
        {
            return score1;
        }
    }

    public static void AddScore(Player playerType)
    {
        if (playerType == Player.Player1)
        {
            if (scoreBoost)
            {
                score2 += 20;
            }
            else
            {
                score2 += 10;
            }
        }

        else
        {
            if (scoreBoost)
            {
                score1 += 20;
            }
            else
            {
                score1 += 10;
            }
        }


    }

    public static void SubtractScore(Player playerType)
    {
        if (playerType == Player.Player1)
        {
            if (score2 > 0)
            {
                score2 -= 10;
            }
        }
        else
        {
            if (score1 > 0)
            {
                score1 -= 10;
            }
        }
    }

    private static void InitializeStatic()
    {
        score1 = 0;
        score2 = 0;
    }
}
