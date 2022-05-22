using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


[Serializable]
public struct MainMenuElement
{
    public String callback;
    public GameObject parent;

    public MainMenuElement(String callback, GameObject parent)
    {
        this.callback = callback;
        this.parent = parent;
    }
}

public class MainMenu : MonoBehaviour
{
    public List<MainMenuElement> mainMenuElements;
    public string firstScene;
    public TitleScreen titleScreen;
    int currentPlayerChoiceIdx = 0;

    void Start()
    {
        mainMenuElements[0].parent.GetComponent<IHighlightableElement>().Highlight();
    }

    // Update is called once per frame
    void Update()
    {
        var previousMenuIdx = currentPlayerChoiceIdx;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentPlayerChoiceIdx = Math.Max(currentPlayerChoiceIdx - 1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentPlayerChoiceIdx = Math.Min(currentPlayerChoiceIdx + 1, mainMenuElements.Count - 1);
        }

        if (currentPlayerChoiceIdx != previousMenuIdx)
        {
            mainMenuElements[previousMenuIdx].parent.GetComponent<IHighlightableElement>().UnHightlight();
            mainMenuElements[currentPlayerChoiceIdx].parent.GetComponent<IHighlightableElement>().Highlight();
            // Play sound
        }


        if (Input.GetAxis("Fire1") == 1)
        {
            Invoke(mainMenuElements[currentPlayerChoiceIdx].callback, 0);
        }
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    void bleh()
    {
        Debug.Log("bleh");
        SceneManager.LoadScene(firstScene);
    }
}
