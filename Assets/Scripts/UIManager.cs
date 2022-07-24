using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    public BossLifeBar bossLifeBar;

    private GameMode gameMode;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        gameMode = GameConfiguration.GetCurrentGameMode();
    }

    public void OnBossReady(Boss boss)
    {
        if (gameMode == GameMode.Challenge)
        {
            bossLifeBar.gameObject.SetActive(true);
            bossLifeBar.AttachBoss(boss);
        }
    }

    public void UpdateBossPhase(int phaseIndex)
    {
        bossLifeBar.SetFillAmount(1);
        bossLifeBar.SetLifeBarIndex(phaseIndex);
    }
}

