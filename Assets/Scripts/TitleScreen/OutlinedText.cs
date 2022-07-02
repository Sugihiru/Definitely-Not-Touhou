using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OutlinedText : MonoBehaviour
{
    TextMeshProUGUI text;
    public Color32 outlineColor;
    public float outlineWidth;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        text.outlineWidth = outlineWidth;
        text.outlineColor = outlineColor;
        text.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, outlineWidth);
    }
}
