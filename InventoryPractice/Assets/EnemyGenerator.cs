using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {
    /// <summary>
    /// Generator
    /// </summary>
    
    float GenerateTime;
    float coolTime;
    
    int temp;
    EnemyPool enemyPool;


    List<GameObject> activeObjects;

    [SerializeField]
    List<Vector3> generatePositions;

    int waveCount;
    int nomalWaveGenerateCount;
    int finalWaveGenerateCount;
    bool IsGenerating;
    bool IsChecking;
    bool IsStageClear;
    public void Initialize(int generatorCount, int waveCount, List<Vector3> positions)
    {
        enemyPool = EnemyManager.instance.enemyPool;
        coolTime = EnemyManager.instance.CoolTime;
        IsGenerating = false;

        generatePositions = positions;
        this.waveCount = waveCount;
        nomalWaveGenerateCount = generatorCount / waveCount;
        finalWaveGenerateCount = nomalWaveGenerateCount + (generatorCount % waveCount);
        activeObjects = new List<GameObject>();
        StartCoroutine(GenerateObject(nomalWaveGenerateCount));
    }
    private void Update()
    {
        if (enemyPool != null)
        {
            StartCoroutine(CheckAliveEnemy());
            if (activeObjects != null && activeObjects.Count<=0)//하나의 웨이브끝
            {
                coolTime -= Time.deltaTime;
                if (waveCount > 1 && !IsGenerating && coolTime <= 0)
                {
                    StartCoroutine(GenerateObject(nomalWaveGenerateCount));
                }
                else if(waveCount == 1 && !IsGenerating && coolTime <= 0)
                {
                    StartCoroutine(GenerateObject(finalWaveGenerateCount));
                }
                else if(waveCount<=0 && !IsStageClear)
                {
                    IsStageClear = true;
                    EnemyManager.instance.CheckClear(true);
                }
            }
        }
    }
    //public static void Invoke(this MonoBehaviour m, Action method, float time)
    //{
    //    m.Invoke(method.Method.Name, time);
    //}
    IEnumerator CheckAliveEnemy()
    {
        while (activeObjects != null && !IsChecking)
        {
            IsChecking = true;
            for (int i = 0; i < activeObjects.Count; i++)
            {
                if (activeObjects[i].activeSelf == false)
                {
                    activeObjects.Remove(activeObjects[i].gameObject);
                    yield return new WaitForSeconds(0.3f);
                }
            }
            IsChecking = false;
            yield return new WaitForSeconds(0.3f);
        }
    }
    IEnumerator GenerateObject(int Count)
    {
        IsGenerating = true;
        waveCount -= 1;
        for (int i = 0; i < Count; i++)
        {
            activeObjects.Add(GetObjectFromPool());
            yield return new WaitForSeconds(1f);
        }
        coolTime = EnemyManager.instance.CoolTime;
        yield return null;
        IsGenerating = false;
    }
    GameObject GetObjectFromPool()
    {
        GameObject tempObject = enemyPool.Pop();
        tempObject.transform.position = generatePositions[UnityEngine.Random.Range(0, generatePositions.Count)];
        tempObject.SetActive(true);
        return tempObject;
    }
}
