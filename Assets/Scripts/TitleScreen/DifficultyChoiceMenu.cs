using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyChoiceMenu : AMenu
{
    public string firstScene;

    void StartGameNormal()
    {
        StartGameWithDifficulty("Normal");
    }

    void StartGameHard()
    {
        StartGameWithDifficulty("Hard");
    }

    void StartGameLunatic()
    {
        StartGameWithDifficulty("Lunatic");
    }

    void StartGameWithDifficulty(string difficulty)
    {
        PlayerPrefs.SetString("Difficulty", difficulty);
        StartGame();
    }

    void StartGame()
    {
        SceneManager.LoadScene(firstScene);
    }
}
