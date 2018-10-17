using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour {
    #region Singleton
    public static EnemyPool instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion
    
    /// <summary>
    /// Pool
    /// </summary>
    public GameObject EnemyPrefab;
    public List<GameObject> PoolList;

    int nomalWaveGenerateCount;
    int finalWaveGenerateCount;

    public void Initialize(int generatorCount, int waveCount, GameObject enemyPrefab)
    {
        PoolList = new List<GameObject>();
        EnemyPrefab = enemyPrefab;
        nomalWaveGenerateCount = generatorCount / waveCount;
        finalWaveGenerateCount = nomalWaveGenerateCount + (generatorCount % waveCount);
        for (int i = 0; i < finalWaveGenerateCount; i++)
        {
            PoolList.Add(CreateObject(EnemyPrefab));
        }
    }

    /// <summary>
    /// Pool System
    /// </summary>
    /// <param name="enemyPrefab"></param>
    /// <returns></returns>
    GameObject CreateObject(GameObject enemyPrefab)
    {
        GameObject tempObject = Instantiate(enemyPrefab);
        tempObject.transform.SetParent(gameObject.transform);
        tempObject.SetActive(false);
        return tempObject;
    }
    public void Push(GameObject enemyObject)
    {
        enemyObject.transform.SetParent(gameObject.transform);
        enemyObject.SetActive(false);
        PoolList.Add(enemyObject);
    }
    public GameObject Pop()
    {
        if(PoolList.Count == 0)
        {
            PoolList.Add(CreateObject(EnemyPrefab));
        }
        GameObject gameObject = PoolList[0];
        PoolList.RemoveAt(0);
        return gameObject;
    }
}
