using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverEndMenuScreen : AMenu<GameOverMenuScreenType>
{
    void Awake()
    {
        onBackKeyPressed = GoToQuitChoice;
    }

    void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void GoBackToMenu()
    {
        SceneManager.LoadScene("Title");
    }

    void GoToQuitChoice()
    {
        UpdateCurrentSelectedMenu(menuElements.Count - 1, playSfx: false);
    }
}
