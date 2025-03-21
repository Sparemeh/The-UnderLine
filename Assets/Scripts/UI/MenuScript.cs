using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void DisplayMenu()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
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

