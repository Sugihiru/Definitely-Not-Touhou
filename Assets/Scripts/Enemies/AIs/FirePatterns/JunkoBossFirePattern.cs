using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkoBossFirePattern : MonoBehaviour
{
    [Header("First phase")]
    public BoxCollider2D firstPatternBulletSpawnZone;
    public GameObject firstPatternBullet;

    [Header("Second phase")]
    public BoxCollider2D secondPatternLeftBulletSpawnZone;
    public BoxCollider2D secondPatternRightBulletSpawnZone;
    public GameObject secondPatternBullet;

    [Header("Third phase")]
    public BoxCollider2D thirdPatternLeftBulletSpawnZone;
    public BoxCollider2D thirdPatternRightBulletSpawnZone;
    public GameObject thirdPatternBullet;

    [Header("Fourth phase")]
    public Transform fourthPatternBulletSpawnCenterPoint;
    public GameObject fourthPatternBullet;

    private float firstCooldownTime = 0;
    private float secondCooldownTime = 0;
    private float thirdCooldownTime = 0;
    private float fourthCooldownTime = 0;
    private Boss bossData;
    private GameObject gameField;

    private const float bulletSpeed = 3f;

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
        thirdCooldownTime -= Time.deltaTime;
        fourthCooldownTime -= Time.deltaTime;

        if (bossData.nbStocks <= 4)
        {
            if (firstCooldownTime < 0)
            {
                Vector2 spawnPoint = RandomPointInBounds(firstPatternBulletSpawnZone.bounds);
                FireCircleSpread(spawnPoint, ObjectPool.SharedInstance.GetFirstPhaseBulletFromPool, numberOfBullets: 24, bulletSpeed: 3f);
                firstCooldownTime = 0.35f;
            }
        }
        if (bossData.nbStocks <= 3)
        {
            if (secondCooldownTime < 0)
            {
                Vector2 spawnPoint = RandomPointInBounds(secondPatternLeftBulletSpawnZone.bounds);
                FireCircleSpread(spawnPoint, ObjectPool.SharedInstance.GetSecondPhaseBulletFromPool, numberOfBullets: 10, bulletSpeed: 2f);
                spawnPoint = RandomPointInBounds(secondPatternRightBulletSpawnZone.bounds);
                FireCircleSpread(spawnPoint, ObjectPool.SharedInstance.GetSecondPhaseBulletFromPool, numberOfBullets: 10, bulletSpeed: 2f);
                secondCooldownTime = 0.65f;
            }
        }

        if (bossData.nbStocks <= 2)
        {
            if (thirdCooldownTime < 0)
            {
                Vector2 spawnPoint = RandomPointInBounds(thirdPatternLeftBulletSpawnZone.bounds);
                FireCircleSpread(spawnPoint, ObjectPool.SharedInstance.GetThirdPhaseBulletFromPool, numberOfBullets: 52, bulletSpeed: 4f);
                spawnPoint = RandomPointInBounds(thirdPatternRightBulletSpawnZone.bounds);
                FireCircleSpread(spawnPoint, ObjectPool.SharedInstance.GetThirdPhaseBulletFromPool, numberOfBullets: 52, bulletSpeed: 4f);
                thirdCooldownTime = 1.5f;
            }
        }

        if (bossData.nbStocks == 1)
        {
            if (fourthCooldownTime < 0)
            {
                Vector2 spawnPoint = fourthPatternBulletSpawnCenterPoint.position;
                FireCircleSpread(spawnPoint, ObjectPool.SharedInstance.GetFourthPhaseBulletFromPool, numberOfBullets: 24, bulletSpeed: 6f);
                fourthCooldownTime = 0.2f;
            }
        }
    }


    private void FireCircleSpread(Vector3 bulletSpawnPosition, Func<GameObject> gameObjectPooler, int numberOfBullets = 16, float bulletSpeed = 2f)
    {
        GameObject bullet;
        const float radius = 0.5f;

        for (var i = 0; i < numberOfBullets; i++)
        {
            var x = bulletSpawnPosition.x + radius * Mathf.Cos(2 * Mathf.PI * i / numberOfBullets);
            var y = bulletSpawnPosition.y + radius * Mathf.Sin(2 * Mathf.PI * i / numberOfBullets);

            bullet = gameObjectPooler();
            bullet.SetActive(true);
            bullet.transform.position = new Vector3(x, y, 0);

            bullet.GetComponent<IParametrableBullet>().speed = bulletSpeed;

            Vector3 direction = bullet.gameObject.transform.position - bulletSpawnPosition;
            bullet.GetComponent<IParametrableBullet>().direction = direction;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private Vector2 RandomPointInBounds(Bounds bounds)
    {
        return new Vector2(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            UnityEngine.Random.Range(bounds.min.y, bounds.max.y)
        );
    }
}
