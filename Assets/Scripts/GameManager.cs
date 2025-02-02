using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public int playerScore { get; private set; }
    public string gameState;
    private GameObject continueEvent;
    private GameObject levelLoader;
    private bool loadLevel = true;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Time.timeScale = 1;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        InitLevel();
    }

    private void Update()
    {
        if (loadLevel == true)
        {
            {
                var loads = GameObject.FindGameObjectsWithTag("load");
                if (loads.Length != 0)
                {
                    levelLoader = loads[0];
                    levelLoader.BroadcastMessage("PrepareToStart");
                }
                var continues = GameObject.FindGameObjectsWithTag("continue");
                if (continues.Length != 0)
                {
                    continueEvent = continues[0];
                    continueEvent.SetActive(false);
                }
                loadLevel = false;
            }
        }
    }

    public void InitLevel()
    {
        loadLevel = true;
        gameState = "initGame";
    }

    public void ChangeLevel()
    {
        levelLoader.BroadcastMessage("ChangeLevel", SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void TriggerContinue()
    {
        Time.timeScale = 0;
        gameState = "continue";
        continueEvent.SetActive(true);
        continueEvent.BroadcastMessage("StartCountdown");
    }

    public void RestartGame()
    {
        continueEvent.SetActive(false);
        playerScore = 0;
        GameObject.FindGameObjectsWithTag("player")[0].BroadcastMessage("Respawn");
        Time.timeScale = 1;
        gameState = "playing";
    }

    public void GameOver()
    {
        gameState = "gameOver";
        Time.timeScale = 0;
        UIManager.instance.ShowGameOverScreen();
        BgmManager.instance.ChangeToGameOverBgm();
    }

    public void AddScore(int scoreToAdd)
    {
        Assert.IsTrue(scoreToAdd > 0);
        playerScore += scoreToAdd;
    }
}

