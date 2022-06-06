using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct MenuElement
{
    public String callback;
    public GameObject parent;

    public MenuElement(String callback, GameObject parent)
    {
        this.callback = callback;
        this.parent = parent;
    }
}

public abstract class AMenu : MonoBehaviour
{
    [SerializeField]
    protected TitleScreen titleScreen;
    [SerializeField]
    protected List<MenuElement> menuElements;
    protected int currentPlayerChoiceIdx = 0;
    protected Action onBackKeyPressed;

    void Start()
    {
        menuElements[0].parent.GetComponent<IHighlightableElement>().Highlight();
    }

    // Update is called once per frame
    void Update()
    {
        var previousMenuIdx = currentPlayerChoiceIdx;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            UpdateCurrentSelectedMenu(Math.Max(currentPlayerChoiceIdx - 1, 0));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            UpdateCurrentSelectedMenu(Math.Min(currentPlayerChoiceIdx + 1, menuElements.Count - 1));
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Invoke(menuElements[currentPlayerChoiceIdx].callback, 0);
        }
        if (Input.GetButtonDown("Bomb"))
        {
            if (onBackKeyPressed != null)
                onBackKeyPressed();
            else
                titleScreen.GoToPreviousMenu();
        }
    }

    protected void UpdateCurrentSelectedMenu(int newPlayerChoiceIdx)
    {
        if (newPlayerChoiceIdx != currentPlayerChoiceIdx)
        {
            menuElements[currentPlayerChoiceIdx].parent.GetComponent<IHighlightableElement>().UnHightlight();
            menuElements[newPlayerChoiceIdx].parent.GetComponent<IHighlightableElement>().Highlight();
            // Play sound

            currentPlayerChoiceIdx = newPlayerChoiceIdx;
        }
    }
}