using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    void Start()
    {
        if (!string.IsNullOrEmpty(LoadingData.sceneToLoad))
            StartCoroutine(LoadSceneAsync(LoadingData.sceneToLoad));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        var asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
