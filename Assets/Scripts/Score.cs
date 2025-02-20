using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int currentScore = 0;
    public TextMeshProUGUI highScoreText;
    public int highScore = 0;

    public void UpdateScore(int scoreToAdd)
    {
        //scoreText.text = currentScore.ToString();
        currentScore += scoreToAdd;
        scoreText.text = $"SCORE:\n{currentScore}";
        
    }

}
