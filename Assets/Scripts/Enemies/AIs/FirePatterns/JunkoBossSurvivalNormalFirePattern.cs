using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkoBossSurvivalNormalFirePattern : AJunkoBossFirePattern
{
    private float firstCooldownTime = 0;
    private float secondCooldownTime = 0;

    private const float bulletSpeed = 3f;


    // Update is called once per frame
    void Update()
    {
        if (!bossData.IsActivated)
            return;

        firstCooldownTime -= Time.deltaTime;
        secondCooldownTime -= Time.deltaTime;

        if (firstCooldownTime < 0)
        {
            Vector2 spawnPoint = RandomPointInBounds(firstPatternBulletSpawnZone.bounds);
            FireCircleSpread(spawnPoint, ObjectPool.SharedInstance.GetFirstPhaseBulletFromPool, numberOfBullets: 22, bulletSpeed: 3f);
            firstCooldownTime = 0.35f;
        }

        if (secondCooldownTime < 0)
        {
            Vector2 spawnPoint = RandomPointInBounds(secondPatternLeftBulletSpawnZone.bounds);
            FireCircleSpread(spawnPoint, ObjectPool.SharedInstance.GetSecondPhaseBulletFromPool, numberOfBullets: 10, bulletSpeed: 2f);
            spawnPoint = RandomPointInBounds(secondPatternRightBulletSpawnZone.bounds);
            FireCircleSpread(spawnPoint, ObjectPool.SharedInstance.GetSecondPhaseBulletFromPool, numberOfBullets: 10, bulletSpeed: 2f);
            secondCooldownTime = 0.65f;
        }
    }
}
