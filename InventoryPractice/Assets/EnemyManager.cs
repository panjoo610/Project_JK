using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(EnemyGenerator),typeof(EnemyPool))]
public class EnemyManager : MonoBehaviour {

    public List<EnemyGenerateData> GenerateDatas;
    public EnemyPool enemyPool;
    public EnemyGenerator enemyGenerator;

    public float CoolTime;
    public int GenerateCount;
    public int WaveCount;
    //public MeshRenderer meshRenderer;

    GameObject EnemyPoolObj;
    GameObject EnemyGeneratorObj;

    public ParticleSystem GenerateParticle;
    public GameObject enemyPrefab;

    public bool IsWorking;

    #region Singleton
    public static EnemyManager instance;

    private void Awake()
    {
        instance = this;
        EnemyPoolObj = new GameObject();
        EnemyGeneratorObj = new GameObject();
        EnemyPoolObj.transform.parent = gameObject.transform;
        EnemyPoolObj.name = "EnemyPoolObj";
        EnemyGeneratorObj.transform.parent = gameObject.transform;
        EnemyGeneratorObj.name = "EnemyGeneratorObj";

        StageManager.instance.OnGameClearCallBack += Initialize;

    }
    #endregion

    // Use this for initialization
    void Start () {
        EnemyPoolObj.AddComponent<EnemyPool>();
        EnemyGeneratorObj.AddComponent<EnemyGenerator>();
        enemyPool = EnemyPoolObj.GetComponent<EnemyPool>();
        enemyGenerator = EnemyGeneratorObj.GetComponent<EnemyGenerator>();
        
    }
    public void ClearStage()
    {
        StageManager.instance.ClearStage();
    }
    public void GenerateEnemy(int currentStage)
    {
        IsWorking = true;
        GenerateCount = GenerateDatas[currentStage-1].EnemyCount;
        WaveCount = GenerateDatas[currentStage-1].WaveCount;
        enemyPool.Initialize(GenerateCount, WaveCount, enemyPrefab);
        enemyGenerator.Initialize(GenerateCount, WaveCount, GenerateDatas[currentStage-1].GeneratePosition);

    }

    void Initialize()
    {
        IsWorking = false;
    }


}
