using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGroup<T> : MonoBehaviour
{
    public List<MenuGroupElement<T>> menuElements;
    Stack<T> previousMenuTypes = new Stack<T>();

    void OnEnable()
    {
        menuElements[0].parent.SetActive(true);
    }

    public void ChangeCurrentMenu(T newMenu, bool isGoingToPreviousMenu = false)
    {
        foreach (var menuElement in menuElements)
        {
            if (!isGoingToPreviousMenu && menuElement.parent.activeInHierarchy)
                previousMenuTypes.Push(menuElement.type);
            menuElement.parent.SetActive(EqualityComparer<T>.Default.Equals(menuElement.type, newMenu));
        }
    }

    public void GoToPreviousMenu()
    {
        if (previousMenuTypes.Count > 0)
            ChangeCurrentMenu(previousMenuTypes.Pop(), true);
    }
}
