using System.Collections;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ScoreModel
{
    public string author { get; set; }
    public int score { get; set; }
    public int seconds_survived { get; set; }
}

public class GameOverMenuScreen : AMenu<GameOverMenuScreenType>
{
    void OnEnable()
    {
        StartCoroutine(UploadScore("https://definitely-not-touhou-api-3jv2rb7i6a-ew.a.run.app/submit-tmp-score"));
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            menuGroup.ChangeCurrentMenu(GameOverMenuScreenType.MainMenu);
        }
    }

    IEnumerator UploadScore(string uri)
    {
        Debug.Log("Uploading score");
        var webRequest = UnityWebRequest.Put(uri, "{\"score\": 20, \"seconds_survived\": 200}");
        webRequest.method = "POST";

        webRequest.SetRequestHeader("Content-Type", "application/json");

        using (webRequest)
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                var scores = JsonConvert.DeserializeObject<List<ScoreModel>>(webRequest.downloadHandler.text);
                Debug.Log(webRequest.downloadHandler.text);
                Debug.Log(scores[0].score);
            }
        }
    }
}
