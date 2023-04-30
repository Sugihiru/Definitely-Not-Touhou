using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;


public class GameOverMenuScreen : AMenu<GameOverMenuScreenType>
{
    public List<ScoreModel> scores;
    public TextMeshProUGUI uploadingScoreText;
    public bool? isHighScore = null;

    void OnEnable()
    {
        if (GameConfiguration.GetCurrentGameMode() == GameMode.Survival)
        {
            StartCoroutine(UploadScore());
        }
        else
        {
            menuGroup.ChangeCurrentMenu(GameOverMenuScreenType.MainMenu);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && isHighScore != null)
        {
            if (isHighScore == false)
            {
                menuGroup.ChangeCurrentMenu(GameOverMenuScreenType.MainMenu);
            }
            else
            {
                menuGroup.ChangeCurrentMenu(GameOverMenuScreenType.Scoreboard);
            }
        }
    }

    public void GoToMainMenu()
    {
        menuGroup.ChangeCurrentMenu(GameOverMenuScreenType.MainMenu);
    }

    IEnumerator UploadScore()
    {
        Debug.Log("Uploading score");
        var webRequest = ScoringApi.GenerateRequestSurvivalSubmitTmpScore(GameManager.instance.playerScore, BattleTimer.timerValue, GameConfiguration.GetCurrentDifficulty());

        using (webRequest)
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.error);
                Debug.Log(webRequest.downloadHandler.text);
                uploadingScoreText.text = "Error while uploading score (HTTP " + webRequest.responseCode + ")";
            }
            else
            {
                // TODO: forward the scores array to the next menu
                this.scores = JsonConvert.DeserializeObject<List<ScoreModel>>(webRequest.downloadHandler.text);

                isHighScore = scores.Any(score => score.tmpScoreId != null);
                Debug.Log(isHighScore);
                uploadingScoreText.text = "Score uploaded";
            }
        }
    }
}
