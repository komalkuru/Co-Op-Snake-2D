using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllSprites : MonoBehaviour
{
    private static AllSprites instance;
    public static AllSprites Instance { get { return instance; } }

    public Sprite SnakeHead1;
    public Sprite SnakeHead2;
    public Sprite EatbleFood;
    public Sprite SnakeBody1;
    public Sprite SnakeBody2;
    public Sprite BurnFood;
    public Sprite Shield;
    public Sprite ScoreBoost;
    public Sprite SpeedUp;

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
    }
}
