using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkoBossFirePattern : MonoBehaviour
{
    public GameObject slowBigBulletGameObject;
    public GameObject simpleParametrableBulletGameObject;
    public GameObject laserBulletGameObject;
    public GameObject bouncingBulletGameObject;

    public Transform middleBulletSpawnPoint;
    public Transform leftBulletSpawnPoint;
    public Transform rightBulletSpawnPoint;

    public BoxCollider2D firstPatternBulletSpawnZone;

    [Header("Bullets")]
    public GameObject firstPatternBullet;


    private float cooldownTime = 0;
    private float secondCooldownTime = 0;
    private int firingCannonIdx;
    private Boss bossData;
    private GameObject gameField;

    private void Awake()
    {
        bossData = GetComponent<Boss>();
        gameField = GameObject.Find("GameField");
        firingCannonIdx = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bossData.IsActivated)
            return;

        cooldownTime -= Time.deltaTime;
        secondCooldownTime -= Time.deltaTime;
        if (bossData.nbStocks == 4)
        {
            if (cooldownTime < 0)
            {
                Vector2 spawnPoint = RandomPointInBounds(firstPatternBulletSpawnZone.bounds);
                FireCircleSpread(spawnPoint, firstPatternBullet, Color.white, numberOfBullets: 28, bulletSpeed: 3f);
                cooldownTime = 0.35f;
            }
        }
        else if (bossData.nbStocks == 3)
        {
            if (cooldownTime < 0)
            {
                var bullet = Instantiate(laserBulletGameObject, middleBulletSpawnPoint.position, Quaternion.identity);
                bullet.GetComponent<SpriteRenderer>().color = Color.red;
                bullet.GetComponent<IParametrableBullet>().speed *= 1.5f;
                cooldownTime = 0.75f;
            }
            if (secondCooldownTime < 0)
            {
                Transform transformCannonToFire;
                if (firingCannonIdx == 0)
                {
                    transformCannonToFire = leftBulletSpawnPoint;
                    firingCannonIdx = 1;
                }
                else
                {
                    transformCannonToFire = rightBulletSpawnPoint;
                    firingCannonIdx = 0;
                }
                StartCoroutine(FireBigCone(transformCannonToFire.position, simpleParametrableBulletGameObject, 80, 0.17f, Color.cyan));
                secondCooldownTime = 2.25f;
            }
        }
        else if (bossData.nbStocks == 2)
        {
            if (cooldownTime < 0)
            {
                // Fire bouncing bullets on the side
                var bullet = Instantiate(bouncingBulletGameObject, leftBulletSpawnPoint.position, Quaternion.identity);
                bullet.GetComponent<IParametrableBullet>().direction = new Vector3(Random.Range(-1f, -0.3f), Random.Range(-1, -0.3f), 0).normalized;
                bullet.GetComponent<SpriteRenderer>().color = Color.green;

                bullet = Instantiate(bouncingBulletGameObject, rightBulletSpawnPoint.position, Quaternion.identity);
                bullet.GetComponent<IParametrableBullet>().direction = new Vector3(Random.Range(0.3f, 1f), Random.Range(-1, -0.3f), 0).normalized;
                bullet.GetComponent<SpriteRenderer>().color = Color.green;

                // Fire three lasers from the main cannon
                // Left one
                bullet = Instantiate(laserBulletGameObject, middleBulletSpawnPoint.position, Quaternion.identity);
                bullet.GetComponent<IParametrableBullet>().direction = new Vector3(Random.Range(-1f, -0.7f), Random.Range(-1, -0.7f), 0).normalized;
                bullet.GetComponent<SpriteRenderer>().color = Color.red;

                // Middle one
                bullet = Instantiate(laserBulletGameObject, middleBulletSpawnPoint.position, Quaternion.identity);
                bullet.GetComponent<IParametrableBullet>().direction = new Vector3(Random.Range(-0.1f, 0.1f), -Random.Range(0.2f, 1f), 0).normalized;
                bullet.GetComponent<SpriteRenderer>().color = Color.red;

                // Right one
                bullet = Instantiate(laserBulletGameObject, middleBulletSpawnPoint.position, Quaternion.identity);
                bullet.GetComponent<IParametrableBullet>().direction = new Vector3(Random.Range(0.7f, 1f), Random.Range(-1, -0.7f), 0).normalized;
                bullet.GetComponent<SpriteRenderer>().color = Color.red;

                cooldownTime = 1f;
            }
        }
        else if (bossData.nbStocks == 1)
        {
            if (cooldownTime < 0)
            {
                FireCircleSpread(middleBulletSpawnPoint.position, simpleParametrableBulletGameObject, Color.blue);
                FireCircleSpread(leftBulletSpawnPoint.position, simpleParametrableBulletGameObject, Color.blue);
                FireCircleSpread(rightBulletSpawnPoint.position, simpleParametrableBulletGameObject, Color.blue);
                cooldownTime = 1f;
            }

            if (secondCooldownTime < 0)
            {
                var bullet = Instantiate(simpleParametrableBulletGameObject, new Vector3(Random.Range(-5.5f, 5.5f), 0, 0) + gameField.transform.position, Quaternion.identity);
                bullet.GetComponent<IParametrableBullet>().direction = new Vector3(0, 1, 0).normalized;
                bullet.GetComponent<IParametrableBullet>().speed *= 0.20f;
                bullet.GetComponent<IParametrableBullet>().ttl = 20f;
                bullet.GetComponent<SpriteRenderer>().color = Color.yellow;
                secondCooldownTime = 0.125f;
            }
        }
    }

    IEnumerator FireBigCone(Vector3 bulletSpawnPosition, GameObject bulletGameObject, int nbBulletsToSpawn, float randomRange, Color color)
    {
        for (int i = 0; i < nbBulletsToSpawn; ++i)
        {
            var bullet = Instantiate(bulletGameObject, bulletSpawnPosition, Quaternion.identity);
            bullet.GetComponent<IParametrableBullet>().direction = new Vector3(
                Random.Range(-randomRange, randomRange),
                -Random.Range(0.2f, 1f), 0
            ).normalized;
            bullet.GetComponent<IParametrableBullet>().speed *= 0.5f;
            bullet.GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(0.0001f);
        }
        yield return null;
    }


    private void FireCircleSpread(Vector3 bulletSpawnPosition, GameObject bulletGameObject, Color color, int numberOfBullets = 16, float bulletSpeed = 2f)
    {
        GameObject bullet;
        const float radius = 0.5f;

        for (var i = 0; i < numberOfBullets; i++)
        {
            var x = bulletSpawnPosition.x + radius * Mathf.Cos(2 * Mathf.PI * i / numberOfBullets);
            var y = bulletSpawnPosition.y + radius * Mathf.Sin(2 * Mathf.PI * i / numberOfBullets);

            bullet = Instantiate(bulletGameObject, new Vector3(x, y, 0), Quaternion.identity);

            bullet.GetComponent<IParametrableBullet>().speed = bulletSpeed;
            bullet.GetComponent<IParametrableBullet>().ttl = 10f;
            bullet.GetComponent<IParametrableBullet>().direction = bullet.gameObject.transform.position - bulletSpawnPosition;
        }
    }

    private Vector2 RandomPointInBounds(Bounds bounds)
    {
        return new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );
    }
}
