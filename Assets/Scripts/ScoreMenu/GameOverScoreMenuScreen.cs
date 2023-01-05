using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GameOverScoreMenuScreen : MonoBehaviour
{
    public GameOverMenuScreen gameOverMenuScreen;
    public TextMeshProUGUI instructionsText;
    public List<ScoreRowDisplay> scoreRowDisplays;

    void OnEnable()
    {
        for (int i = 0; i < scoreRowDisplays.Count; ++i)
        {
            scoreRowDisplays[i].DisplayScore(gameOverMenuScreen.scores[i]);
        }
    }


    public IEnumerator PublishScore(String scoreId, String username)
    {
        Debug.Log("Publishing score");
        instructionsText.text = "Uploading score...";
        var webRequest = UnityWebRequest.Put(
            "https://definitely-not-touhou-api-3jv2rb7i6a-ew.a.run.app/publish-score/" + scoreId,
            "{\"username\": \"" + username + "\"}"
        );
        webRequest.method = "POST";
        webRequest.SetRequestHeader("Content-Type", "application/json");

        using (webRequest)
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.error);
                Debug.Log(webRequest.downloadHandler.text);
            }

            gameOverMenuScreen.GoToMainMenu();
        }
    }
}
