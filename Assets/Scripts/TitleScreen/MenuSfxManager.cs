using System;
using System.Collections.Generic;
using UnityEngine;

public enum MenuSfxType
{
    ChangeSelection,
    ConfirmSelection,
    Cancel,
};

[Serializable]
public struct MenuSfxElement
{
    public MenuSfxType type;
    public AudioClip clip;
}

public class MenuSfxManager : MonoBehaviour
{
    public List<MenuSfxElement> sfxElements;

    private AudioSource audioSource;
    private Dictionary<MenuSfxType, AudioClip> sfxTypeToClip = new Dictionary<MenuSfxType, AudioClip>();

    void Start()
    {
        if (Debug.isDebugBuild)
        {
            var sfxTypes = (MenuSfxType[])Enum.GetValues(typeof(MenuSfxType));
            foreach (var sfxType in sfxTypes)
            {
                var nbFound = sfxElements.FindAll((sfxElement) => sfxElement.type == sfxType).Count;
                if (nbFound != 1)
                {
                    throw new Exception($"Invalid menu SFX elements : key {sfxType} found {nbFound} times instead of 1");
                }
            }
        }

        audioSource = GetComponent<AudioSource>();
        foreach (MenuSfxElement sfxElement in sfxElements)
        {
            sfxTypeToClip.Add(sfxElement.type, sfxElement.clip);
        }
    }

    public void PlaySfx(MenuSfxType sfxType)
    {
        audioSource.PlayOneShot(sfxTypeToClip[sfxType], 0.5f);
    }
}
