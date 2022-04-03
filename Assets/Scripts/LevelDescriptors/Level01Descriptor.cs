using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level01Descriptor : MonoBehaviour
{
    [SerializeField]
    public List<EnemySpawnData> enemiesSpawnData = new List<EnemySpawnData>();
    public EnemySpawnData bossSpawnData;

    public GameObject gameField;
    public SpriteRenderer backgroundField;

    public void LevelStart()
    {
        foreach (var enemySpawnData in enemiesSpawnData)
        {
            StartCoroutine(SpawnEnemy(enemySpawnData));
        }
        StartCoroutine(SpawnBoss(bossSpawnData));
    }

    public IEnumerator SpawnEnemy(EnemySpawnData enemySpawnData)
    {
        yield return new WaitForSeconds(enemySpawnData.spawnTime);
        var enemyGameObject = Instantiate(enemySpawnData.enemyGameObject, enemySpawnData.spawnPosition + gameField.transform.position, Quaternion.identity);

        var enemyMovementBehaviorScript = System.Type.GetType(enemySpawnData.enemyMovementBehaviorScriptName);
        enemyGameObject.AddComponent(enemyMovementBehaviorScript);
        enemyGameObject.transform.parent = gameField.transform;
    }

    public IEnumerator SpawnBoss(EnemySpawnData bossSpawnData)
    {
        yield return new WaitForSeconds(bossSpawnData.spawnTime);
        var enemyGameObject = Instantiate(bossSpawnData.enemyGameObject, bossSpawnData.spawnPosition + gameField.transform.position, Quaternion.identity);

        var enemyMovementBehaviorScript = System.Type.GetType(bossSpawnData.enemyMovementBehaviorScriptName);
        enemyGameObject.AddComponent(enemyMovementBehaviorScript);
        enemyGameObject.transform.parent = gameField.transform;

        // Change background on boss spawn
        if (bossSpawnData.background != null) {
            backgroundField.sprite = bossSpawnData.background;
        }
    }
}
