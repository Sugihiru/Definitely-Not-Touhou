using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighlightableElementText : MonoBehaviour, IHighlightableElement
{
    TextMeshProUGUI text;
    public Color32 initialColor;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void Highlight()
    {
        text.outlineWidth = 0.25f;
        text.outlineColor = initialColor;
        text.color = new Color32(255, 255, 255, 255);
    }

    public void UnHightlight()
    {
        text.outlineWidth = 0;
        text.color = initialColor;
    }
}
