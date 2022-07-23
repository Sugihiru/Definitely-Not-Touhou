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
        var difficulty = PlayerPrefs.GetString("Difficulty");
        var gameMode = PlayerPrefs.GetString("GameMode");

        var enemyGameObject = Instantiate(bossSpawnData.enemyGameObject, bossSpawnData.spawnPosition + gameField.transform.position, Quaternion.identity);

        var firePatternScript = System.Type.GetType($"JunkoBoss{gameMode}{difficulty}FirePattern");
        enemyGameObject.AddComponent(firePatternScript);

        if (gameMode == "Survival")
        {
            var bossBehavior = enemyGameObject.GetComponent<Boss>();
            bossBehavior.isInvincible = true;
            bossBehavior.displayBarLife = false;
        }

        enemyGameObject.transform.parent = gameField.transform;

        // Change background on boss spawn
        if (bossSpawnData.background != null)
        {
            backgroundField.sprite = bossSpawnData.background;
        }
    }
}
