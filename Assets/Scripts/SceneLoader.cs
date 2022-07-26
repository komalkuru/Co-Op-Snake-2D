using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject gameHandler;
    [SerializeField] private SnakeController snake;
    [SerializeField] private FoodSpawner food;

    public enum Scene
    {
        MainMenu,
        SinglePlayer
    }

    public void Menu()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        SceneManager.LoadScene(Scene.MainMenu.ToString());
        Destroy(gameHandler);
    }

    public void Load()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        snake.ResetGame();
        SceneManager.LoadScene(Scene.SinglePlayer.ToString());
    }
}
