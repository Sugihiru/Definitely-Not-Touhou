using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;

    [Header("Junko first phase bullets")]
    [HideInInspector]
    public List<GameObject> firstPhaseBulletsPool;
    public GameObject firstPhaseBulletPrefab;
    public int firstPhaseBulletAmountInPool;

    [Header("Junko second phase bullets")]
    [HideInInspector]
    public List<GameObject> secondPhaseBulletsPool;
    public GameObject secondPhaseBulletPrefab;
    public int secondPhaseBulletAmountInPool;

    [Header("Junko third phase bullets")]
    [HideInInspector]
    public List<GameObject> thirdPhaseBulletsPool;
    public GameObject thirdPhaseBulletPrefab;
    public int thirdPhaseBulletAmountInPool;

    [Header("Junko fourth phase bullets")]
    [HideInInspector]
    public List<GameObject> fourthPhaseBulletsPool;
    public GameObject fourthPhaseBulletPrefab;
    public int fourthPhaseBulletAmountInPool;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        InitPool(ref firstPhaseBulletsPool, firstPhaseBulletPrefab, firstPhaseBulletAmountInPool);
        InitPool(ref secondPhaseBulletsPool, secondPhaseBulletPrefab, secondPhaseBulletAmountInPool);
        InitPool(ref thirdPhaseBulletsPool, thirdPhaseBulletPrefab, thirdPhaseBulletAmountInPool);
        InitPool(ref fourthPhaseBulletsPool, fourthPhaseBulletPrefab, fourthPhaseBulletAmountInPool);
    }

    void Update()
    {
        // Debug.Log("Active in pool" + fourthPhaseBulletsPool.FindAll((x) => x.activeInHierarchy).Count);
    }

    private void InitPool(ref List<GameObject> pool, GameObject gameObjectToCreate, int amount)
    {
        GameObject tmp;

        pool = new List<GameObject>(amount);
        for (int i = 0; i < amount; i++)
        {
            tmp = Instantiate(gameObjectToCreate);
            tmp.SetActive(false);
            pool.Add(tmp);
        }
    }

    public GameObject GetFirstPhaseBulletFromPool()
    {
        return GetObjectFromPool(firstPhaseBulletsPool);
    }

    public GameObject GetSecondPhaseBulletFromPool()
    {
        return GetObjectFromPool(secondPhaseBulletsPool);
    }

    public GameObject GetThirdPhaseBulletFromPool()
    {
        return GetObjectFromPool(thirdPhaseBulletsPool);
    }

    public GameObject GetFourthPhaseBulletFromPool()
    {
        return GetObjectFromPool(fourthPhaseBulletsPool);
    }

    GameObject GetObjectFromPool(List<GameObject> pool)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }
        Debug.Log("Failed to get object from pool");
        GameObject newObject = Instantiate(pool[0]);
        pool.Add(newObject);
        return newObject;
    }
}
