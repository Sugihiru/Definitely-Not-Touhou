using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    public static BgmManager instance = null;
    public AudioClip defaultBgm;
    public AudioClip gameOverBgm;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = defaultBgm;
    }

    public void ChangeToGameOverBgm()
    {
        audioSource.clip = gameOverBgm;
        audioSource.Play();
    }
}
