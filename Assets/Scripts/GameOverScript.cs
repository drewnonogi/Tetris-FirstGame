using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class GameOverScript : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;

    public TextMeshProUGUI GameOverText;
    [field: SerializeField]
    private Score scoreInstance { get; set; }
    [field: SerializeField]
    private BlockController blockControllerInstance;

    public int HiScore;

    [field: SerializeField]
    public TextMeshProUGUI HiScoreText;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(0);
        });
    }

    public void HiScoreUpdate()
    {
        HiScore = PlayerPrefs.GetInt("HiScore");
        if (scoreInstance.currentScore > HiScore)
        {
            PlayerPrefs.SetInt("HiScore", scoreInstance.currentScore);
            HiScore = PlayerPrefs.GetInt("HiScore");

        }

        HiScoreText.text = $"Best score:\n {HiScore}";
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

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) == true)
        {
            Application.Quit();
        }
    }
}
