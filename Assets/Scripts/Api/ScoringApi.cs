using UnityEngine.Networking;

public class ScoringApi
{
    static string baseUrl = "https://definitely-not-touhou-api-3jv2rb7i6a-ew.a.run.app";

    static public UnityWebRequest GenerateRequestSurvivalPublishScore(string scoreId, string username)
    {
        var webRequest = UnityWebRequest.Put(
            ScoringApi.baseUrl + "/survival/publish-score/" + scoreId,
            "{\"username\": \"" + username + "\"}"
        );
        webRequest.method = "POST";
        webRequest.SetRequestHeader("Content-Type", "application/json");
        return webRequest;
    }

    static public UnityWebRequest GenerateRequestSurvivalSubmitTmpScore(int playerScore, float secondsSurvived, Difficulty difficulty)
    {
        var webRequest = UnityWebRequest.Put(
            ScoringApi.baseUrl + "/survival/submit-tmp-score",
            "{\"score\": " + playerScore + ", \"seconds_survived\": " + secondsSurvived.ToString("0.00").Replace(",", ".") + ", \"difficulty\": \"" + difficulty.ToString() + "\"}"
        );
        webRequest.method = "POST";
        webRequest.SetRequestHeader("Content-Type", "application/json");
        return webRequest;
    }
}
