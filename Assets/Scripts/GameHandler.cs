using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameHandler : MonoBehaviour 
{
    private static GameHandler instance;

    [SerializeField] private SnakeController snakeController;

    private LevelGrid levelGrid;

    private void Awake()
    {
        instance = this;
        Score.InitializeStatic();

        Score.TrySetNewHighscore(0);
    }
    private void Start() {
        Debug.Log("GameHandler.Start");

        levelGrid = new LevelGrid(20, 20);

        snakeController.Setup(levelGrid);
        levelGrid.Setup(snakeController);
    }
    public static void SnakeDied()
    {
        Score.TrySetNewHighscore();
    }
}
