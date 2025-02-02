using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    public BossLifeBar bossLifeBar;
    public BattleTimer battleTimer;
    public GameOverMenuGroup gameOverMenuGroup;

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
        else if (gameMode == GameMode.Survival)
        {
            battleTimer.gameObject.SetActive(true);
        }
    }

    public void UpdateBossPhase(int phaseIndex)
    {
        bossLifeBar.SetFillAmount(1);
        bossLifeBar.SetLifeBarIndex(phaseIndex);
    }

    public void ShowGameOverScreen()
    {
        gameOverMenuGroup.gameObject.SetActive(true);
    }
}

