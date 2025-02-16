using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreText;
    public int currentScore = 0;
    public Text highScoreText;
    public int highScore = 0;

    public void UpdateScore(int scoreToAdd)
    {
        //scoreText.text = currentScore.ToString();
        currentScore += scoreToAdd;
        scoreText.text = $"{currentScore}";
        
    }

}
