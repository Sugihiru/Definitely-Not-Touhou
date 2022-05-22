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

    public void ChangeCurrentMenu(MenuScreenType newMenu)
    {
        foreach (var menuElement in menuElements)
        {
            menuElement.parent.SetActive(menuElement.type == newMenu);
        }
    }
}
