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

    [Header("First phase")]
    public BoxCollider2D firstPatternBulletSpawnZone;
    public GameObject firstPatternBullet;

    [Header("Second phase")]
    public BoxCollider2D secondPatternLeftBulletSpawnZone;
    public BoxCollider2D secondPatternRightBulletSpawnZone;
    public GameObject secondPatternBullet;


    private float firstCooldownTime = 0;
    private float secondCooldownTime = 0;
    private Boss bossData;
    private GameObject gameField;

    private void Awake()
    {
        bossData = GetComponent<Boss>();
        gameField = GameObject.Find("GameField");
    }

    // Update is called once per frame
    void Update()
    {
        if (!bossData.IsActivated)
            return;

        firstCooldownTime -= Time.deltaTime;
        secondCooldownTime -= Time.deltaTime;

        if (bossData.nbStocks <= 4)
        {
            if (firstCooldownTime < 0)
            {
                Vector2 spawnPoint = RandomPointInBounds(firstPatternBulletSpawnZone.bounds);
                FireCircleSpread(spawnPoint, firstPatternBullet, Color.white, numberOfBullets: 28, bulletSpeed: 3f);
                firstCooldownTime = 0.35f;
            }
        }

        if (bossData.nbStocks <= 3)
        {
            if (secondCooldownTime < 0)
            {
                Vector2 spawnPoint = RandomPointInBounds(secondPatternLeftBulletSpawnZone.bounds);
                FireCircleSpread(spawnPoint, secondPatternBullet, Color.white, numberOfBullets: 10, bulletSpeed: 3f);
                spawnPoint = RandomPointInBounds(secondPatternRightBulletSpawnZone.bounds);
                FireCircleSpread(spawnPoint, secondPatternBullet, Color.white, numberOfBullets: 10, bulletSpeed: 3f);
                secondCooldownTime = 0.5f;
            }
        }
        else if (bossData.nbStocks == 2)
        {
            if (firstCooldownTime < 0)
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

                firstCooldownTime = 1f;
            }
        }
        else if (bossData.nbStocks == 1)
        {
            if (firstCooldownTime < 0)
            {
                FireCircleSpread(middleBulletSpawnPoint.position, simpleParametrableBulletGameObject, Color.blue);
                FireCircleSpread(leftBulletSpawnPoint.position, simpleParametrableBulletGameObject, Color.blue);
                FireCircleSpread(rightBulletSpawnPoint.position, simpleParametrableBulletGameObject, Color.blue);
                firstCooldownTime = 1f;
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
