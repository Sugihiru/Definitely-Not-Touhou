using System;
using UnityEngine;

public static class GameConfiguration
{
    public static GameMode GetCurrentGameMode()
    {
        return (GameMode)Enum.Parse(typeof(GameMode), PlayerPrefs.GetString("GameMode"));
    }

    public static void SetGameMode(GameMode gameMode)
    {
        PlayerPrefs.SetString("GameMode", gameMode.ToString());
    }

    public static Difficulty GetCurrentDifficulty()
    {
        return (Difficulty)Enum.Parse(typeof(Difficulty), PlayerPrefs.GetString("Difficulty"));
    }

    public static void SetDifficulty(Difficulty difficulty)
    {
        PlayerPrefs.SetString("Difficulty", difficulty.ToString());
    }
}
