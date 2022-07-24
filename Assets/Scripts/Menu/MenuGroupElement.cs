using System;
using UnityEngine;

[Serializable]
public struct MenuGroupElement<T>
{
    public T type;
    public GameObject parent;

    public MenuGroupElement(T type, GameObject parent)
    {
        this.type = type;
        this.parent = parent;
    }
}