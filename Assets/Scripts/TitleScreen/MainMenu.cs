using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : AMenu
{
    public string firstScene;
    public TitleScreen titleScreen;

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
}
