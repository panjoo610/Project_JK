using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyGenerator),typeof(EnemyPool))]
public class EnemyManager : MonoBehaviour {
    #region Singleton
    public static EnemyManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public List<EnemyGenerateData> GenerateDatas;
    public EnemyPool enemyPool;
    public EnemyGenerator enemyGenerator;

    public float CoolTime;
    public int GenerateCount;
    public int WaveCount;
    //public MeshRenderer meshRenderer;

    // Use this for initialization
    void Start () {
        enemyPool = GetComponent<EnemyPool>();
        enemyGenerator = GetComponent<EnemyGenerator>();
	}
    public void CheckClear(bool check)
    {
        if (check)
        {
            StageManager.instance.ClearStage();
        }
    }
    public void GenerateEnemy(int currentStage)
    {
        GenerateCount = GenerateDatas[currentStage].EnemyCount;
        WaveCount = GenerateDatas[currentStage].WaveCount;
        enemyPool.Initialize(GenerateCount);
        enemyGenerator.Initialize(GenerateCount, WaveCount, GenerateDatas[currentStage].GeneratePosition);

    }
}
