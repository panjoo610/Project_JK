﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GenerateData
{
    public bool StartAwake;
    public ParticleSystem GenerateParticle;

    public EnemyType enemyType;
    public float DelayTime;
    public int MaxCount;

    public bool IsSpread;
    public float SpreadRange;

    public bool IsLoop;
    public float CoolTime;
    public bool IsUnlimited;
}

public class Generator : GeneratorController
{

    public bool UsingInspectorData;
    public GenerateData generateData;

    ParticleSystem GenerateParticle;

    EnemyType enemyType;
    float DelayTime;
    int MaxCount;

    bool IsSpread;
    float SpreadRange;

    bool IsLoop;
    float CoolTime;
    bool IsUnlimited;

    GameObject generateObject;

    IEnumerator Coroutine;
    bool IsOver;

    List<GameObject> activeObjects;
    int AliveCount;

    public delegate void OnGenerateOver();
    public OnGenerateOver OnGenerateOverCallBack;

    protected override void Start()
    {
        if (UsingInspectorData == true)
        {
            GenerateParticle = generateData.GenerateParticle;
            enemyType = generateData.enemyType;
            DelayTime = generateData.DelayTime;
            MaxCount = generateData.MaxCount;

            IsSpread = generateData.IsSpread;
            SpreadRange = generateData.SpreadRange;

            IsLoop = generateData.IsLoop;
            CoolTime = generateData.CoolTime;
            IsUnlimited = generateData.IsUnlimited;
            if (IsUnlimited == true)
            {
                activeObjects = new List<GameObject>();
            }
            if (generateData.StartAwake == true)
            {
                StartGenerating();
            }
        }
    }


    public void Initialize(EnemyType enemyType, float DelayTime, int MaxCount)
    {
        GenerateParticle = GetComponentInChildren<ParticleSystem>();
        IsOver = false;
        IsSpread = false;
        IsLoop = false;
        this.enemyType = enemyType;
        this.DelayTime = DelayTime;
        this.MaxCount = MaxCount;
    }
    public void Initialize(EnemyType enemyType, float DelayTime, int MaxCount, float SpreadRange)
    {
        Initialize(enemyType, DelayTime, MaxCount);
        IsSpread = true;
        this.SpreadRange = SpreadRange;
    }
    public void Initialize(EnemyType enemyType, float DelayTime, int MaxCount, float SpreadRange, float CoolTime, bool IsUnlimited)
    {
        Initialize(enemyType, DelayTime, MaxCount, SpreadRange);
        IsLoop = true;
        this.CoolTime = CoolTime;
        this.IsUnlimited = IsUnlimited;
        if (IsUnlimited == true)
        {
            activeObjects = new List<GameObject>();
        }
    }

    public void StartGenerating()
    {
        Coroutine = Generate();
        StartCoroutine(Coroutine);
    }
    public void StopGenerating()
    {
        StopCoroutine(Coroutine);
        Coroutine = Generate();
        IsOver = true;
    }

    public bool CheckSelf()
    {
        return IsOver;
    }

    IEnumerator Generate()
    {
        int GenerateCount = 0;
        AliveCount = 0;
        while (GenerateCount < MaxCount)
        {
            GenerateCount++;

            generateObject = GetObjectFromPool(enemyType);
            if (IsSpread == true)
            {
                Vector3 newVector = Spread(SpreadRange);
                GenerateParticle.transform.position = newVector;
                GenerateParticle.Play();
                generateObject.transform.position = newVector;
            }
            else
            {
                GenerateParticle.transform.position = gameObject.transform.position;
                GenerateParticle.Play();
                generateObject.transform.position = gameObject.transform.position;
            }
            yield return new WaitForSeconds(1f);
            generateObject.SetActive(true);

            if (IsLoop == true && IsUnlimited == false)
            {
                activeObjects.Add(generateObject);
                AliveCount++;
            }
            yield return new WaitForSeconds(DelayTime);
        }

        if (IsLoop == true)
        {
            if (IsUnlimited == false)
            {
                StartCoroutine(CheckAliveObject(activeObjects));
                while (AliveCount > 0)
                {
                    yield return new WaitForSeconds(DelayTime);
                } 
            }
            yield return new WaitForSeconds(CoolTime);
            StartGenerating();
        }
        else
            IsOver = true;
        yield return null;
    }

    IEnumerator CheckAliveObject(List<GameObject> aliveObjects)
    {
        Debug.Log("CheckAliveEnemy");
        while (aliveObjects != null)
        {
            for (int i = 0; i < aliveObjects.Count; i++)
            {
                if (aliveObjects[i].activeSelf == false)
                {
                    AliveCount--;
                    //EnemyManager.instance.ChangeEnemyleftCount(1);
                    aliveObjects.Remove(aliveObjects[i].gameObject);
                    yield return new WaitForSeconds(0.3f);
                }
            }
            yield return new WaitForSeconds(0.3f);
        }
        GenerateOver();
    }

    Vector3 Spread(float SpreadRange)
    {
        Vector3 newVector = new Vector3(
            Random.Range(-SpreadRange + gameObject.transform.position.x, SpreadRange + gameObject.transform.position.x),
            gameObject.transform.position.y,
            Random.Range(-SpreadRange + gameObject.transform.position.z, SpreadRange + gameObject.transform.position.z));
        return newVector;
    }

    GameObject GetObjectFromPool(EnemyType enemyType)
    {
        GameObject tempObject = EnemyManager.instance.enemyPool.Pop(enemyType);
        //tempObject.SetActive(true);
        return tempObject;
    }


    public void GenerateOver()
    {
        if (OnGenerateOverCallBack != null)
            OnGenerateOverCallBack.Invoke();
    }
}
