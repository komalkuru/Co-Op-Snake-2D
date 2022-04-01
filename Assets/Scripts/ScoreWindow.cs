using UnityEngine;
using TMPro;

public class ScoreWindow : MonoBehaviour {

    private TextMeshProUGUI scoreText;

    private void Awake() 
    {
        scoreText = transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        scoreText.text = "Score: " + GameHandler.GetScore().ToString();
    }
}
