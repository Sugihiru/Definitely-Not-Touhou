using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyChoiceMenu : AMenu
{
    public string firstScene;

    void StartGameNormal()
    {
        StartGameWithDifficulty(Difficulty.Normal);
    }

    void StartGameHard()
    {
        StartGameWithDifficulty(Difficulty.Hard);
    }

    void StartGameLunatic()
    {
        StartGameWithDifficulty(Difficulty.Lunatic);
    }

    void StartGameWithDifficulty(Difficulty difficulty)
    {
        PlayerPrefs.SetString("Difficulty", difficulty.ToString());
        StartGame();
    }

    void StartGame()
    {
        LoadingData.sceneToLoad = firstScene;
        SceneManager.LoadScene("LoadingScene");
    }
}
