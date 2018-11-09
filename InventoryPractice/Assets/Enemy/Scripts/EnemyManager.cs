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
    public bool IsBossStage;
    //public MeshRenderer meshRenderer;

    GameObject EnemyPoolObj;
    GameObject EnemyGeneratorObj;

    public ParticleSystem GenerateParticle;
    public GameObject enemyPrefab;
    public GameObject bossPrefab;

    public bool IsWorking;

    public int currentStage;
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
    public void GenerateEnemy(int currentStage)
    {
        this.currentStage = currentStage - 1;
        GenerateDatas[this.currentStage].Initialize();
        
        IsWorking = true;
        GenerateCount = GenerateDatas[this.currentStage].EnemyCount;
        WaveCount = GenerateDatas[this.currentStage].WaveCount;
        IsBossStage = GenerateDatas[this.currentStage].IsBossStage;
        if (IsBossStage == false)
        {
            enemyPool.Initialize(GenerateCount, WaveCount, enemyPrefab);
            enemyGenerator.Initialize(GenerateCount, WaveCount, GenerateDatas[this.currentStage].GeneratePosition);
        }
        else
        {
            enemyPool.Initialize(GenerateCount, WaveCount, enemyPrefab, bossPrefab);
            enemyGenerator.Initialize(GenerateCount, WaveCount, GenerateDatas[this.currentStage].GeneratePosition, true);
        }
    }

    public void Initialize()
    {
        IsWorking = false;
    }

    public void ClearStage()
    {
        StageManager.instance.ClearStage();
        GenerateDatas[currentStage].Initialize();
        enemyPool.ClearPool();
    }

    public void StageExit()
    {
        GenerateDatas[currentStage].Initialize();
        IsWorking = false;
        Debug.Log("강제종료");
        enemyGenerator.StoppingGenerating();
        enemyPool.ClearPool();
    }


    

}
