using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelJunkoDescriptor : MonoBehaviour
{
    public EnemySpawnData bossSpawnData;

    public GameObject gameField;
    public SpriteRenderer backgroundField;

    public void LevelStart()
    {
        StartCoroutine(SpawnBoss(bossSpawnData));
    }


    public IEnumerator SpawnBoss(EnemySpawnData bossSpawnData)
    {
        yield return new WaitForSeconds(bossSpawnData.spawnTime);
        var difficulty = GameConfiguration.GetCurrentDifficulty();
        var gameMode = GameConfiguration.GetCurrentGameMode();

        var enemyGameObject = Instantiate(bossSpawnData.enemyGameObject, bossSpawnData.spawnPosition + gameField.transform.position, Quaternion.identity);

        var firePatternScript = System.Type.GetType($"JunkoBoss{gameMode}{difficulty}FirePattern");
        enemyGameObject.AddComponent(firePatternScript);

        if (gameMode == GameMode.Survival)
        {
            var bossBehavior = enemyGameObject.GetComponent<Boss>();
            bossBehavior.isInvincible = true;
        }

        enemyGameObject.transform.parent = gameField.transform;

        // Change background on boss spawn
        if (bossSpawnData.background != null)
        {
            backgroundField.sprite = bossSpawnData.background;
        }
    }
}
