using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : AMenu<TitleScreenMenuScreenType>
{
    public string firstScene;

    void Awake()
    {
        onBackKeyPressed = GoToQuitChoice;
    }

    void EnterSurvivalModeDifficultyChoice()
    {
        GameConfiguration.SetGameMode(GameMode.Survival);
        menuGroup.ChangeCurrentMenu(TitleScreenMenuScreenType.DifficultyChoiceMenu);
    }

    void ExtraStartGame()
    {
        GameConfiguration.SetGameMode(GameMode.Challenge);
        LoadingData.sceneToLoad = firstScene;
        SceneManager.LoadScene("LoadingScene");
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
