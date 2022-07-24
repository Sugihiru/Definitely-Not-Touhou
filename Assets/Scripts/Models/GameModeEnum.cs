using System;
using UnityEngine;

public enum GameMode
{
    Survival,
    Challenge,
};

public static class GameModeExtension
{
    public static GameMode GetCurrentGameMode()
    {
        return (GameMode)Enum.Parse(typeof(GameMode), PlayerPrefs.GetString("GameMode"));
    }
}