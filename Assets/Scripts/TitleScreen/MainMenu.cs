using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : AMenu
{
    public string firstScene;

    void Awake()
    {
        onBackKeyPressed = GoToQuitChoice;
    }

    void EnterSurvivalModeDifficultyChoice()
    {
        PlayerPrefs.SetString("GameMode", "Survival");
        titleScreen.ChangeCurrentMenu(MenuScreenType.DifficultyChoiceMenu);
    }

    void ExtraStartGame()
    {
        PlayerPrefs.SetString("GameMode", "Extra");
        SceneManager.LoadScene(firstScene);
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    void GoToQuitChoice()
    {
        UpdateCurrentSelectedMenu(menuElements.Count - 1, playSfx: false);
    }
}
