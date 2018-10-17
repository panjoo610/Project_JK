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
    bool runingCoroutine;

    public void Initialize(int generatorCount, int waveCount, List<Vector3> positions)
    {
        enemyPool = EnemyManager.instance.enemyPool;
        coolTime = EnemyManager.instance.CoolTime;
        runingCoroutine = false;

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
            if (activeObjects != null && activeObjects.Count<=0)//하나의 웨이브끝
            {
                coolTime -= Time.deltaTime;
                if (waveCount > 1 && !runingCoroutine && coolTime <= 0)
                {
                    StartCoroutine(GenerateObject(nomalWaveGenerateCount));
                }
                else if(waveCount == 1 && !runingCoroutine && coolTime <= 0)
                {
                    StartCoroutine(GenerateObject(finalWaveGenerateCount));
                }
                else
                {
                    EnemyManager.instance.CheckClear(true);
                }
            }
        }
    }
    //public static void Invoke(this MonoBehaviour m, Action method, float time)
    //{
    //    m.Invoke(method.Method.Name, time);
    //}
    IEnumerator GenerateObject(int Count)
    {
        runingCoroutine = true;
        waveCount -= 1;
        for (int i = 0; i < Count; i++)
        {
            activeObjects.Add(GetObjectFromPool());
            yield return new WaitForSeconds(1f);
        }
        coolTime = EnemyManager.instance.CoolTime;
        yield return null;
        runingCoroutine = false;
    }
    GameObject GetObjectFromPool()
    {
        GameObject tempObject = enemyPool.Pop();
        tempObject.transform.position = generatePositions[UnityEngine.Random.Range(0, generatePositions.Count)];
        tempObject.SetActive(true);
        return tempObject;
    }
}
