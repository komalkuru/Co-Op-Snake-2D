using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MultipleScoreController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText1;
    [SerializeField] private TextMeshProUGUI scoreText2;
    [SerializeField] private TextMeshProUGUI highScore1;
    [SerializeField] private TextMeshProUGUI highScore2;
    [SerializeField] private MultipleSnakeController snake1;
    [SerializeField] private MultipleSnakeController snake2;
    private int score1;
    private int score2;

    private void Awake()
    {
        scoreText1 = transform.Find("ScoreText1").GetComponent<TextMeshProUGUI>();
        scoreText2 = transform.Find("ScoreText2").GetComponent<TextMeshProUGUI>();

        highScore1.text = "Highscore: " + PlayerPrefs.GetInt("HighScore1").ToString();
        highScore2.text = "Highscore: " + PlayerPrefs.GetInt("HighScore2").ToString();
    }

    private void Update()
    {
        scoreText1.text = MultipleGameHandler.GetScore(snake1.playerType).ToString();
        scoreText2.text = MultipleGameHandler.GetScore(snake2.playerType).ToString();

        score1 = MultipleGameHandler.GetScore(snake1.playerType);
        score2 = MultipleGameHandler.GetScore(snake2.playerType);

        if (score1 > PlayerPrefs.GetInt("HighScore1"))
        {
            SetHighScore1();
        }
        
        if (score2 > PlayerPrefs.GetInt("HighScore2"))
        {
            SetHighScore2();
        }
    }

    private void SetHighScore1()
    {
        PlayerPrefs.SetInt("HighScore1", score1);
        highScore1.text = "Highscore: " + PlayerPrefs.GetInt("HighScore1").ToString();
    }

    private void SetHighScore2()
    {
        PlayerPrefs.SetInt("HighScore2", score2);
        highScore2.text = "Highscore: " + PlayerPrefs.GetInt("HighScore2").ToString();
    }
}
