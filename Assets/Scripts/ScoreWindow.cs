using UnityEngine;
using TMPro;

public class ScoreWindow : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScore;
    [SerializeField] private SnakeController snake;
    private int score;

    private void Awake() 
    {
        scoreText = transform.Find("Score").GetComponent<TextMeshProUGUI>();
        
        highScore.text = "Highscore:" + PlayerPrefs.GetInt("HighScore").ToString();
    }

    private void Update()
    {
        scoreText.text = GameHandler.GetScore().ToString();

        score = GameHandler.GetScore();

        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            SetHighScore();
        }
    }

    private void SetHighScore()
    {
        PlayerPrefs.SetInt("HighScore", score);
        highScore.text = "Highscore:" + PlayerPrefs.GetInt("HighScore").ToString();
    }
}
