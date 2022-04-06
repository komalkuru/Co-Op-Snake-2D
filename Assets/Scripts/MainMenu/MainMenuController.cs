using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button aboutGameButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject ChooseOption;
    [SerializeField] private Button singlePlayer;
    [SerializeField] private Button multiPlayer;
    public GameObject PlayGameObject;
    [SerializeField] private Button playButton;
    public string sceneName;
    


    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        singlePlayer.onClick.AddListener(PlaySinglePlayerGame);
        playButton.onClick.AddListener(PlayGame);
    }

    public void StartGame()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        ChooseOption.SetActive(true);
    }
    public void PlaySinglePlayerGame()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        PlayGameObject.SetActive(true);
    }

    public void PlayGame()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        SceneManager.LoadScene(sceneName);
    }
}
