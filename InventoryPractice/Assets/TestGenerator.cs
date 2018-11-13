using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGenerator : MonoBehaviour {

    public ParticleSystem GenerateParticle;
    EnemyType enemyType;
    GameObject generateObject;
    float DelayTime;
    int MaxCount;

    bool IsSpread;
    float SpreadRange;

    bool IsLoop;
    float CoolTime;
    bool IsUnlimited;

    IEnumerator Coroutine;
    bool IsOver;

    List<GameObject> activeObjects;
    int AliveCount;

    public void Initialize(EnemyType enemyType, float DelayTime, int MaxCount)
    {
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
        while (GenerateCount <= MaxCount)
        {
            GenerateCount++;

            generateObject = GetObjectFromPool(enemyType);
            if (IsSpread == true)
            {
                Vector3 newVector = Spread(SpreadRange);
                GenerateParticle.gameObject.transform.position = newVector;
                GenerateParticle.Play();
                generateObject.transform.position = newVector;
            }
            else
            {
                GenerateParticle.gameObject.transform.position = gameObject.transform.position;
                GenerateParticle.Play();
                generateObject.transform.position = gameObject.transform.position;
            }
            yield return new WaitForSeconds(0.3f);
            generateObject.SetActive(true);

            if (IsUnlimited == true)
            {
                activeObjects.Add(generateObject);
                AliveCount++;
            }
            yield return new WaitForSeconds(DelayTime);
        }
        if (IsLoop == true)
        {
            if (IsUnlimited == true)
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

    IEnumerator CheckAliveObject(List<GameObject> activeObjects)
    {
        Debug.Log("CheckAliveEnemy");
        while (activeObjects != null)
        {
            for (int i = 0; i < activeObjects.Count; i++)
            {
                if (activeObjects[i].activeSelf == false)
                {
                    AliveCount--;
                    //EnemyManager.instance.ChangeEnemyleftCount(1);
                    activeObjects.Remove(activeObjects[i].gameObject);
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
        //tempObject.SetActive(true);
        return tempObject;
    }
    

}
