using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public GameObject normalShotBullet;
    public GameObject focusedShotBullet;
    public AudioSource audioSourceFire;
    public Transform bulletSpawnPoint;

    public float normalShotFireDelay;
    public float focusedShotFireDelay;
    public bool canFire = true;

    private float normalShotCooldown = 0;
    private float focusedShotCooldown = 0;


    // Update is called once per frame
    void Update()
    {
        normalShotCooldown -= Time.deltaTime;
        focusedShotCooldown -= Time.deltaTime;

        if (Input.GetAxis("Fire1") == 1 && canFire)
        {
            if (normalShotCooldown < 0)
            {
                Instantiate(normalShotBullet, bulletSpawnPoint.position, transform.rotation);
                audioSourceFire.Play();
                normalShotCooldown = normalShotFireDelay;
            }
            if (focusedShotCooldown < 0)
            {
                Instantiate(focusedShotBullet, bulletSpawnPoint.position, transform.rotation);
                focusedShotCooldown = focusedShotFireDelay;
            }
        }
    }
}
