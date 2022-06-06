using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct TitleScreenMenuElement
{
    public MenuScreenType type;
    public GameObject parent;

    public TitleScreenMenuElement(MenuScreenType type, GameObject parent)
    {
        this.type = type;
        this.parent = parent;
    }
}

public class TitleScreen : MonoBehaviour
{
    public List<TitleScreenMenuElement> menuElements;
    Stack<MenuScreenType> previousMenuTypes = new Stack<MenuScreenType>();

    public void ChangeCurrentMenu(MenuScreenType newMenu, bool isGoingToPreviousMenu = false)
    {
        foreach (var menuElement in menuElements)
        {
            if (!isGoingToPreviousMenu && menuElement.parent.activeInHierarchy)
                previousMenuTypes.Push(menuElement.type);
            menuElement.parent.SetActive(menuElement.type == newMenu);
        }
    }

    public void GoToPreviousMenu()
    {
        if (previousMenuTypes.Count > 0)
            ChangeCurrentMenu(previousMenuTypes.Pop(), true);
    }
}
