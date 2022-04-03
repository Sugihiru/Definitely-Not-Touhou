using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct BossPhaseDescriptor
{
    public int maxHealth;

    public BossPhaseDescriptor(Sprite sprite, int maxHealth)
    {
        this.maxHealth = maxHealth;
    }
}


public class Boss : EnemyDamaged
{
    public GameObject scoreCollectible;
    public List<BossPhaseDescriptor> phaseDescriptors = new List<BossPhaseDescriptor>();
    public int nbStocks = 3;
    public bool IsActivated { get; private set; }
    public AudioClip phaseClip;

    private BossLifeBar bossLifeBar;
    private int maxNbStocks;

    // Start is called before the first frame update
    void Start()
    {
        maxNbStocks = nbStocks;
        // Find BossLifeBar GameObject by name, even if it's disabled
        BossLifeBar[] objs = Resources.FindObjectsOfTypeAll<BossLifeBar>();
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == "BossLifeBar")
                {
                    bossLifeBar = objs[i];
                    break;
                }
            }
        }
    }

    public new void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsActivated)
            return;

        if (other.tag == "playerBullet")
        {
            health -= other.GetComponent<PlayerBulletBehavior>().damage;
            bossLifeBar.SetFillAmount((float)base.health / (float)base.maxHealth);
            if (health <= 0)
            {
                RemoveStock();
            }
            Destroy(other.gameObject);
        }
    }

    private void RemoveStock()
    {
        nbStocks -= 1;
        if (nbStocks == 0)
        {
            base.DestroyOnKill();
            ConvertAllBulletsToScore();
            GameManager.instance.ChangeLevel();
            return;
        }
        else if (nbStocks < 0)
        {
            return;
        }

        int idx = maxNbStocks - nbStocks - 1;
        SetMaxHealth(phaseDescriptors[idx].maxHealth);
        StartCoroutine(BossPhaseChanging(idx, nbStocks));
    }

    // Enable lifebar + enable firing
    public void Activate()
    {
        bossLifeBar.gameObject.SetActive(true);
        IsActivated = true;
    }

    private void ConvertAllBulletsToScore()
    {
        foreach (var bullet in GameObject.FindGameObjectsWithTag("enemyBullet"))
        {
            Instantiate(scoreCollectible, bullet.transform.position, Quaternion.identity);
            Destroy(bullet);
        }
    }

    IEnumerator BossPhaseChanging(int idx, int nbStocks)
    {
        // Use "PlayClipAtPoint" to avoid create an AudioSource and to avoid managing the Destroyed state
        gameObject.GetComponent<Animator>().SetTrigger("Touch");
        AudioSource.PlayClipAtPoint(phaseClip, gameObject.transform.position, 0.5f);
        yield return new WaitForSecondsRealtime(0.5f);
        SetMaxHealth(phaseDescriptors[idx].maxHealth);
        bossLifeBar.SetFillAmount(1);
        bossLifeBar.SetLifeBarIndex(nbStocks);
    }
}
