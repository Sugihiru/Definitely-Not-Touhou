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
    [SerializeField]
    protected MenuSfxManager menuSfxManager;

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
            menuSfxManager.PlaySfx(MenuSfxType.ConfirmSelection);
            Invoke(menuElements[currentPlayerChoiceIdx].callback, 0);
        }
        if (Input.GetButtonDown("Bomb"))
        {
            menuSfxManager.PlaySfx(MenuSfxType.Cancel);
            if (onBackKeyPressed != null)
                onBackKeyPressed();
            else
                titleScreen.GoToPreviousMenu();
        }
    }

    protected void UpdateCurrentSelectedMenu(int newPlayerChoiceIdx, bool playSfx = true)
    {
        if (newPlayerChoiceIdx != currentPlayerChoiceIdx)
        {
            menuElements[currentPlayerChoiceIdx].parent.GetComponent<IHighlightableElement>().UnHightlight();
            menuElements[newPlayerChoiceIdx].parent.GetComponent<IHighlightableElement>().Highlight();

            if (playSfx)
                menuSfxManager.PlaySfx(MenuSfxType.ChangeSelection);

            currentPlayerChoiceIdx = newPlayerChoiceIdx;
        }
    }
}