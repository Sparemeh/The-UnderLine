using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    [SerializeField]
    TMP_Text scoreText;
    public void DisplayMenu(int score)
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        scoreText.text = "Score: " + score;
    }

    public void HideMenu()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        HideMenu();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
