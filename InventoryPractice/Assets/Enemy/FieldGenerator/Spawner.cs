using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpawnData
{
    public bool IsConfirmBeforeClear;

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

public class Spawner : AbstractMapController
{
    public bool UsingInspectorData;
    public SpawnData generateData;

    ParticleSystem GenerateParticle;

    EnemyType enemyType;
    float DelayTime;
    int MaxCount;

    bool IsSpread;
    float SpreadRange;

    bool IsLoop;
    float CoolTime;
    bool IsUnlimited;
    bool IsConfirmBeforeClear;

    GameObject generateObject;

    IEnumerator generateEnumerator;
    Coroutine co;
    bool IsOver;

    List<GameObject> activeObjects;
    int AliveCount;

    protected override void Start()
    {
        generateEnumerator = Generate();
        EnemyManager.instance.OnStageExitEvent += StoppingGenerating;
        if (UsingInspectorData == true)
        {
            IsConfirmBeforeClear = generateData.IsConfirmBeforeClear;
            GenerateParticle = generateData.GenerateParticle;
            enemyType = generateData.enemyType;
            DelayTime = generateData.DelayTime;
            MaxCount = generateData.MaxCount;

            IsSpread = generateData.IsSpread;
            SpreadRange = generateData.SpreadRange;

            IsLoop = generateData.IsLoop;
            CoolTime = generateData.CoolTime;
            IsUnlimited = generateData.IsUnlimited;

            if (IsLoop == true || IsConfirmBeforeClear == true)
            {
                activeObjects = new List<GameObject>();
            }
            if (generateData.StartAwake == true)
            {
                StartGenerating();
            }
            EnemyManager.instance.ChangeEnemyleftCount(MaxCount);
        }

    }


    public void Initialize(bool IsConfirmBeforeClear, EnemyType enemyType, float DelayTime, int MaxCount)
    {
        GenerateParticle = GetComponentInChildren<ParticleSystem>();
        IsOver = false;
        IsSpread = false;
        IsLoop = false;
        this.IsConfirmBeforeClear = IsConfirmBeforeClear;
        this.enemyType = enemyType;
        this.DelayTime = DelayTime;
        this.MaxCount = MaxCount;
        EnemyManager.instance.ChangeEnemyleftCount(MaxCount);
    }
    public void Initialize(bool IsConfirmBeforeClear, EnemyType enemyType, float DelayTime, int MaxCount, float SpreadRange)
    {
        Initialize(IsConfirmBeforeClear, enemyType, DelayTime, MaxCount);
        IsSpread = true;
        this.SpreadRange = SpreadRange;
    }
    public void Initialize(bool IsConfirmBeforeClear, EnemyType enemyType, float DelayTime, int MaxCount, float SpreadRange, float CoolTime, bool IsUnlimited)
    {
        Initialize(IsConfirmBeforeClear, enemyType, DelayTime, MaxCount, SpreadRange);
        IsLoop = true;
        this.CoolTime = CoolTime;
        this.IsUnlimited = IsUnlimited;
        if (IsUnlimited == false)
        {
            activeObjects = new List<GameObject>();
        }
    }

    public void StartGenerating()
    {
        co = StartCoroutine(generateEnumerator);
    }
    public void StopGenerating()
    {
        if (co != null)
        {
            StopCoroutine(co);
            IsOver = true; 
        }
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

            if (IsLoop == true && IsUnlimited == false || IsConfirmBeforeClear == true)
            {
                activeObjects.Add(generateObject);
                AliveCount++;
            }
            yield return new WaitForSeconds(DelayTime);
            GenerateCount++;
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
        else if (IsConfirmBeforeClear == true)
        {
            StartCoroutine(CheckAliveObject(activeObjects));
            while (AliveCount > 0)
            {
                yield return new WaitForSeconds(DelayTime);
            }
            IsOver = true;
            SendReport();
        }
        else
        {
            IsOver = true;
            SendReport();
        }
        yield return null;
    }

    IEnumerator CheckAliveObject(List<GameObject> aliveObjects)
    {
        while (aliveObjects != null)
        {
            for (int i = 0; i < aliveObjects.Count; i++)
            {
                if (aliveObjects[i].activeSelf == false)
                {
                    AliveCount--;
                    aliveObjects.Remove(aliveObjects[i].gameObject);
                    yield return new WaitForSeconds(0.3f);
                }
            }
            yield return new WaitForSeconds(0.3f);
        }
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
        return tempObject;
    }

    protected override void SendReport()
    {
        base.SendReport();
    }

    protected override void TakeReport()
    {
        return;
    }


    public void StoppingGenerating()
    {
        if (co != null)
        {
            StopGenerating();
            for (int i = 0; i < activeObjects.Count; i++)
            {
                activeObjects[i].SetActive(false);
                EnemyManager.instance.enemyPool.Push(activeObjects[i]);
            } 
        }
        EnemyManager.instance.OnStageExitEvent -= StoppingGenerating;
        
    }
}
