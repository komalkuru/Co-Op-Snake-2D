using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiGameOverController : MonoBehaviour
{
    public WinText winText;
    public MultipleSnakeController snake1;
    public MultipleSnakeController snake2;
    public GameOverWindow gameOver;

    private int score1;
    private int score2;
    private bool gameOverBool = false;

    private void Update()
    {
        score1 = MultipleGameHandler.GetScore(Player.Player1);
        score2 = MultipleGameHandler.GetScore(Player.Player2);

        if (!gameOverBool)
        {
            CheckWinCondition();
        }
    }

    public void CheckWinCondition()
    {
        if (score1 >= 100)
        {
            gameOverBool = true;
            StartCoroutine(ResetBool());
            winText.SetWinText(1);
            snake1.state = State.Dead;
            snake2.state = State.Dead;
            gameOver.GameOver();
        }
        else if (score2 >= 100)
        {
            gameOverBool = true;
            StartCoroutine(ResetBool());
            winText.SetWinText(1);
            snake1.state = State.Dead;
            snake2.state = State.Dead;
            gameOver.GameOver();
        }
    }

    public IEnumerator ResetBool()
    {
        yield return new WaitUntil(() => score1 == 0 && score2 == 0);
        gameOverBool = false;
        yield break;
    }
}
