using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public Text GameOverText;
    [field: SerializeField]
    private Score scoreInstance { get; set; }
    [field: SerializeField]
    private BlockController blockControllerInstance;
    //[field: SerializeField]
    public int HiScore;

    [field: SerializeField]
    public Text HiScoreText;


    public void HiScoreUpdate()
    {
        if (scoreInstance.currentScore > HiScore)
        {
            PlayerPrefs.SetInt("HiScore", scoreInstance.currentScore);
        }
        HiScore = PlayerPrefs.GetInt("HiScore");
        HiScoreText.text = $"{HiScore}";
    }


    public void GameOver()
    {
        HiScoreUpdate();
        gameObject.SetActive(true);
        GameOverText.text = $"Lines cleared in this run: {scoreInstance.currentScore}";
        Time.timeScale = 0.0f;

    }
    public void TryAgainButton()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Main");
    }
}
