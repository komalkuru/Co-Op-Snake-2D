using UnityEngine;
using TMPro;
using System;

public class ScoreWindow : MonoBehaviour {

    private TextMeshProUGUI scoreText;

    private void Awake() 
    {
        scoreText = transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();

        Score.OnHighscoreChanged += Score_OnHighscoreChanged;
        UpdateHighScore();
    }

    private void Score_OnHighscoreChanged(object sender, EventArgs e)
    {
        UpdateHighScore();
    }

    private void Update()
    {
        scoreText.text = "Score: " + Score.GetScore().ToString();
    }

    private void UpdateHighScore()
    {
        int highscore = Score.GetHighscore();
        transform.Find("HighscoreText").GetComponent<TextMeshProUGUI>().text = "Highscore: " + highscore.ToString();
    }
}
