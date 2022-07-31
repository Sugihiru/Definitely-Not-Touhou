using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct BossPhaseDescriptor
{
    public int maxHealth;
    public bool destroyBulletsOnEnteringPhase;

    public BossPhaseDescriptor(Sprite sprite, int maxHealth, bool destroyBulletsOnEnteringPhase)
    {
        this.maxHealth = maxHealth;
        this.destroyBulletsOnEnteringPhase = destroyBulletsOnEnteringPhase;
    }
}


public class Boss : EnemyDamaged
{
    public List<BossPhaseDescriptor> phaseDescriptors = new List<BossPhaseDescriptor>();
    public int nbStocks = 3;
    public bool IsActivated { get; private set; }
    public AudioClip phaseClip;

    private int maxNbStocks;

    // Start is called before the first frame update
    void Start()
    {
        maxNbStocks = nbStocks;
    }

    public new void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsActivated)
            return;

        if (other.tag == "playerBullet")
        {
            int dmg = other.GetComponent<PlayerBulletBehavior>().damage;
            GameManager.instance.AddScore(dmg);
            if (!isInvincible)
            {
                health -= dmg;
                if (health <= 0)
                {
                    RemoveStock();
                }
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
            GameManager.instance.ChangeLevel();
            return;
        }
        else if (nbStocks < 0)
        {
            return;
        }

        int idx = maxNbStocks - nbStocks - 1;
        if (phaseDescriptors[idx].destroyBulletsOnEnteringPhase)
        {
            DestroyAllBullets();
        }
        SetMaxHealth(phaseDescriptors[idx].maxHealth);
        BossPhaseChanging(idx, nbStocks);
    }

    // Enable lifebar + enable firing
    public void Activate()
    {
        UIManager.instance.OnBossReady(this);
        IsActivated = true;
    }

    public float GetBossLifeRatio()
    {
        return (float)base.health / (float)base.maxHealth;
    }

    private void DestroyAllBullets()
    {
        foreach (var bullet in GameObject.FindGameObjectsWithTag("enemyBullet"))
        {
            bullet.SetActive(false);
        }
    }

    private void BossPhaseChanging(int idx, int nbStocks)
    {
        // Use "PlayClipAtPoint" to avoid create an AudioSource and to avoid managing the Destroyed state
        gameObject.GetComponent<Animator>().SetTrigger("Touch");
        AudioSource.PlayClipAtPoint(phaseClip, gameObject.transform.position, 0.5f);
        SetMaxHealth(phaseDescriptors[idx].maxHealth);
        UIManager.instance.UpdateBossPhase(nbStocks);
    }
}
