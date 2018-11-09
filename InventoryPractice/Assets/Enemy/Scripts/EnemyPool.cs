using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Nomal, Elite, Boss
}
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
    GameObject enemyPrefab;
    GameObject bossPrefab;
    public List<GameObject> PoolList;
    public List<GameObject> BossPoolList;

    EnemyType enemyType;

    int nomalWaveGenerateCount;
    int finalWaveGenerateCount;
    
    public void Initialize(int generatorCount, int waveCount, GameObject enemyPrefab)
    {
        PoolList = new List<GameObject>();
        BossPoolList = new List<GameObject>();
        this.enemyPrefab = enemyPrefab;
        nomalWaveGenerateCount = generatorCount / waveCount;
        finalWaveGenerateCount = nomalWaveGenerateCount + (generatorCount % waveCount);
        for (int i = 0; i < finalWaveGenerateCount; i++)
        {
            PoolList.Add(CreateObject(this.enemyPrefab));
        }
    }
    public void Initialize(int generatorCount, int waveCount, GameObject enemyPrefab, GameObject bossPrefab)
    {
        Initialize(generatorCount, waveCount, enemyPrefab);
        this.bossPrefab = bossPrefab;
        BossPoolList.Add(CreateObject(bossPrefab));
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
    public void Push(GameObject pushObject)
    {
        if (pushObject == enemyPrefab)
        {
            pushObject.transform.SetParent(gameObject.transform);
            pushObject.SetActive(false);
            PoolList.Add(pushObject);
        }
        else
        {
            pushObject.transform.SetParent(gameObject.transform);
            pushObject.SetActive(false);
            BossPoolList.Add(pushObject);
        }
    }
    public GameObject Pop(EnemyType enemyType)
    {
        List<GameObject> useList;
        GameObject usePrefab;
        switch (enemyType)
        {
            case EnemyType.Nomal:
                useList = PoolList;
                usePrefab = enemyPrefab;
                break;
            case EnemyType.Boss:
                useList = BossPoolList;
                usePrefab = bossPrefab;
                break;
            default:
                useList = PoolList;
                usePrefab = enemyPrefab;
                break;
        }
        if(useList.Count == 0)
        {
            useList.Add(CreateObject(usePrefab));
        }
        GameObject PopObject = useList[0];
        useList.RemoveAt(0);
        return PopObject;
    }
    public void ClearPool()
    {
        foreach (GameObject item in PoolList)
        {
            Destroy(item);
        }
        foreach (GameObject item in BossPoolList)
        {
            Destroy(item);
        }
        PoolList.Clear();
        BossPoolList.Clear();
    }
}
