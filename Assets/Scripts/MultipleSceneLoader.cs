using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultipleSceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject gameHandler;
    [SerializeField] private MultipleSnakeController snake1;
    [SerializeField] private MultipleSnakeController snake2;

    public enum Scene
    {
        MainMenu,
        MultiPlayer
    }

    public void Load()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        snake1.ResetGame();
        snake2.ResetGame();
        SceneManager.LoadScene(Scene.MultiPlayer.ToString());
    }

    public void Menu()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        SceneManager.LoadScene(Scene.MainMenu.ToString());
        Destroy(gameHandler);
    }
}
