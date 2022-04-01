using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameHandler : MonoBehaviour 
{

    private static GameHandler instance;
    public static int score;

    [SerializeField] private SnakeController snakeController;

    private LevelGrid levelGrid;

    private void Awake()
    {
        instance = this;
    }
    private void Start() {
        Debug.Log("GameHandler.Start");

        levelGrid = new LevelGrid(20, 20);

        snakeController.Setup(levelGrid);
        levelGrid.Setup(snakeController);
    }

    public static int GetScore()
    {
        return score;
    }
    public static void AddScore()
    {
        score += 10;
    }
}
