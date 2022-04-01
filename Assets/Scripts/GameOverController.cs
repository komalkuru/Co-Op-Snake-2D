﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button mainMenuButton;
    public string mainSceneName;
    public string singlePlayerSceneName;



    private void Start()
    {
        retryButton.onClick.AddListener(PlayGame);
        mainMenuButton.onClick.AddListener(PlaySinglePlayerGame);        
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(singlePlayerSceneName);
    }

    public void PlaySinglePlayerGame()
    {
        SceneManager.LoadScene(mainSceneName);
    }
}