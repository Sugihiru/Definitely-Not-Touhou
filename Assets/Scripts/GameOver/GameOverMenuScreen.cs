using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class ScoreModel
{
    public string author { get; set; }
    public int score { get; set; }
    public float secondsSurvived { get; set; }
    public string tmpScoreId { get; set; }

    public override string ToString()
    {
        return "From=" + author + ";Score=" + score + ";SecondsSurvived=" + secondsSurvived;
    }
}

public class GameOverMenuScreen : AMenu<GameOverMenuScreenType>
{
    public TextMeshProUGUI uploadingScoreText;
    public bool? isHighScore = null;

    void OnEnable()
    {
        StartCoroutine(UploadScore("https://definitely-not-touhou-api-3jv2rb7i6a-ew.a.run.app/submit-tmp-score"));
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

    IEnumerator UploadScore(string uri)
    {
        Debug.Log("Uploading score");
        var webRequest = UnityWebRequest.Put(uri, "{\"score\": " + GameManager.instance.playerScore + ", \"seconds_survived\": " + BattleTimer.timerValue.ToString("0.00").Replace(",", ".") + "}");
        webRequest.method = "POST";
        webRequest.SetRequestHeader("Content-Type", "application/json");

        using (webRequest)
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.error);
                Debug.Log(webRequest.downloadHandler.text);
                uploadingScoreText.text = "Error while uploading text (HTTP " + webRequest.responseCode + ")";
            }
            else
            {
                // TODO: forward the scores array to the next menu
                var scores = JsonConvert.DeserializeObject<List<ScoreModel>>(webRequest.downloadHandler.text);

                Debug.Log(webRequest.downloadHandler.text);

                isHighScore = scores.Any(score => score.tmpScoreId != null);
                Debug.Log(isHighScore);
                uploadingScoreText.text = "Score uploaded";
            }
        }
    }
}
