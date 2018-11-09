﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {
    /// <summary>
    /// Generator
    /// </summary>

    //float coolTime;
    int currentEnemyCount;
    EnemyPool enemyPool;

    ParticleSystem GenerateParticle;


    List<GameObject> activeObjects;

    [SerializeField]
    List<Vector3> generatePositions;

    int waveCount;
    int nomalWaveGenerateCount;
    int finalWaveGenerateCount;
    bool IsBossStage;
    bool IsGenerating;
    bool IsChecking;
    bool IsStageClear;

    IEnumerator EnemyGenerate;
    IEnumerator nomalWaveGenerate;
    IEnumerator finalWaveGenerate;
    IEnumerator CheckEnemy;

    public void Initialize(int generatorCount, int waveCount, List<Vector3> positions)
    {
        GenerateParticle = EnemyManager.instance.GenerateParticle;
        enemyPool = EnemyManager.instance.enemyPool;
        //coolTime = EnemyManager.instance.CoolTime;
        IsStageClear = false;
        IsChecking = false;
        IsGenerating = false;
        generatePositions = positions;
        this.waveCount = waveCount;
        nomalWaveGenerateCount = generatorCount / waveCount;
        finalWaveGenerateCount = nomalWaveGenerateCount + (generatorCount % waveCount);
        activeObjects = new List<GameObject>();

        currentEnemyCount = generatorCount;

        CheckEnemy = CheckAliveEnemy();
        nomalWaveGenerate = GenerateObject(nomalWaveGenerateCount,false);
        finalWaveGenerate = GenerateObject(finalWaveGenerateCount,true);
        StartCoroutine(nomalWaveGenerate);
        EnemyGenerate = Generating();
        

        StartCoroutine(EnemyGenerate);
    }
    public void Initialize(int generatorCount, int waveCount, List<Vector3> positions, bool isBossStage)
    {
        Initialize(generatorCount, waveCount, positions);
        this.IsBossStage = isBossStage;
    }

    IEnumerator Generating()
    {
        while (EnemyManager.instance.IsWorking)
        {
            if (enemyPool != null)
            {
                yield return null;
                StartCoroutine(CheckEnemy);
                if (activeObjects != null && activeObjects.Count <= 0)//하나의 웨이브끝
                {
                    Debug.Log("웨이브 시작 "+waveCount + " 제네레이팅");
                    if (waveCount > 1 && IsGenerating == false)
                    {
                        Debug.Log(waveCount + " 제네레이팅");
                        nomalWaveGenerate = GenerateObject(nomalWaveGenerateCount, IsBossStage);
                        StartCoroutine(nomalWaveGenerate);
                    }
                    else if (waveCount == 1 && IsGenerating == false)
                    {
                        Debug.Log(waveCount + " 제네레이팅");
                        //StartCoroutine(GenerateObject(finalWaveGenerateCount));
                        StartCoroutine(finalWaveGenerate);
                        finalWaveGenerate = GenerateObject(finalWaveGenerateCount, IsBossStage);
                    }
                    else if (waveCount <= 0 && !IsStageClear)
                    {
                        Debug.Log(waveCount + " 제네레이팅");
                        ClearStage(true);
                    }
                }
                yield return null;
            }
            yield return new WaitForSeconds(0.4f);
        }
    }

    /// <summary>
    /// 모든 적을 제거하면 스테이지를 클리어 했다고 알린다
    /// </summary>
    /// <param name="check"></param>
    private void ClearStage(bool check)
    {
        if (check)
        {
            IsStageClear = true;
            EnemyManager.instance.ClearStage();        }
    }

    //public static void Invoke(this MonoBehaviour m, Action method, float time)
    //{
    //    m.Invoke(method.Method.Name, time);
    //}


    /// <summary>
    /// 생성된 적을 담고있는 리스트에서 죽은 적을 계속 체크하여 리스트에서 빼준다
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckAliveEnemy()
    {
        while (activeObjects != null && !IsChecking)
        {
            if (IsGenerating == false)
            {
                IsChecking = true;
                for (int i = 0; i < activeObjects.Count; i++)
                {
                    if (activeObjects[i].activeSelf == false)
                    {
                        currentEnemyCount--;
                        EnemyManager.instance.GenerateDatas[EnemyManager.instance.currentStage].currentCount = currentEnemyCount;
                        Debug.Log(EnemyManager.instance.GenerateDatas[EnemyManager.instance.currentStage].currentCount);
                        activeObjects.Remove(activeObjects[i].gameObject);
                        yield return new WaitForSeconds(0.3f);
                    }
                }
                IsChecking = false;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    /// <summary>
    /// 한개의 웨이브의 적 수만큼 적을 만들어낸다
    /// </summary>
    /// <param name="Count"></param>
    /// <returns></returns>
    IEnumerator GenerateObject(int Count, bool isBossGenerate)
    {
        Debug.Log(IsGenerating);
        IsGenerating = true;
        Debug.Log(IsGenerating);
        for (int i = 0; i < Count; i++)
        {
            yield return new WaitForSeconds(2f);
            if (EnemyManager.instance.IsWorking)
            {
                Vector3 newTransform = generatePositions[UnityEngine.Random.Range(0, generatePositions.Count)];

                yield return null;
                ParticleSystem test = Instantiate(GenerateParticle);
                test.gameObject.transform.position = newTransform;
                test.Play();

                yield return new WaitForSeconds(.7f);
                if (EnemyManager.instance.IsWorking)
                {
                    Debug.Log(Count);
                    if (isBossGenerate == true && i == Count - 1)
                    {
                        activeObjects.Add(GetObjectFromPool(newTransform, EnemyType.Boss));
                    }
                    else
                    {
                        activeObjects.Add(GetObjectFromPool(newTransform, EnemyType.Nomal));
                    }
                }
            }
        }
        waveCount -= 1;
        yield return null;
        IsGenerating = false;
        //StopCoroutine(finalWaveGenerate);
    }


    /// <summary>
    /// 풀에서 오브젝트를 꺼내서 반환해준다
    /// </summary>
    /// <param name="newTransform"></param>
    /// <returns></returns>
    GameObject GetObjectFromPool(Vector3 newTransform, EnemyType enemyType)
    {
        GameObject tempObject = enemyPool.Pop(enemyType);
        tempObject.transform.position = newTransform;
        tempObject.SetActive(true);
        return tempObject;
    }

    /// <summary>
    /// 생성을 멈추고 생성되있는 오브젝트를 모두 풀에 반환한다
    /// </summary>
    public void StoppingGenerating()
    {
        StopCoroutine(EnemyGenerate);
        StopCoroutine(nomalWaveGenerate);
        StopCoroutine(finalWaveGenerate);
        StopCoroutine(CheckEnemy);
        enemyPool = null;
        waveCount = 0;
        for (int i = 0; i < activeObjects.Count; i++)
        {
            activeObjects[i].SetActive(false);
            EnemyManager.instance.enemyPool.Push(activeObjects[i]);
        }
        CheckEnemy = CheckAliveEnemy();
        EnemyGenerate = Generating();
        nomalWaveGenerate = GenerateObject(nomalWaveGenerateCount,false);
        finalWaveGenerate = GenerateObject(finalWaveGenerateCount,true);
    }

}
