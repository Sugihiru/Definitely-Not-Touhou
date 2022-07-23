using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AJunkoBossFirePattern : MonoBehaviour
{
    [Header("First phase")]
    protected BoxCollider2D firstPatternBulletSpawnZone;

    [Header("Second phase")]
    protected BoxCollider2D secondPatternLeftBulletSpawnZone;
    protected BoxCollider2D secondPatternRightBulletSpawnZone;

    [Header("Third phase")]
    protected BoxCollider2D thirdPatternLeftBulletSpawnZone;
    protected BoxCollider2D thirdPatternRightBulletSpawnZone;

    [Header("Fourth phase")]
    protected Transform fourthPatternBulletSpawnCenterPoint;

    protected Boss bossData;

    protected virtual void Awake()
    {
        bossData = GetComponent<Boss>();
        firstPatternBulletSpawnZone = transform.Find("Pattern1ZoneBulletSpawnPoint").GetComponent<BoxCollider2D>();
        secondPatternLeftBulletSpawnZone = transform.Find("Pattern2LeftZoneBulletSpawnPoint").GetComponent<BoxCollider2D>();
        secondPatternRightBulletSpawnZone = transform.Find("Pattern2RightZoneBulletSpawnPoint").GetComponent<BoxCollider2D>();
        thirdPatternLeftBulletSpawnZone = transform.Find("Pattern3LeftZoneBulletSpawnPoint").GetComponent<BoxCollider2D>();
        thirdPatternRightBulletSpawnZone = transform.Find("Pattern3RightZoneBulletSpawnPoint").GetComponent<BoxCollider2D>();
        fourthPatternBulletSpawnCenterPoint = transform.Find("Pattern4BulletSpawnCenterPoint").transform;
    }

    protected void FireCircleSpread(Vector3 bulletSpawnPosition, Func<GameObject> gameObjectPooler, int numberOfBullets = 16, float bulletSpeed = 2f)
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

    protected Vector2 RandomPointInBounds(Bounds bounds)
    {
        return new Vector2(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            UnityEngine.Random.Range(bounds.min.y, bounds.max.y)
        );
    }
}
