using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : AbstractMapController
{
    [SerializeField]
    Spawner[] generators;
    Spawner Generator;
    int generatorCount;
    int clearCount;
    bool isWorked;

    // Use this for initialization
    protected override void Start() {
        base.Start();
        Initialize();
    }
	void Initialize()
    {
        Generator = GetComponentInChildren<Spawner>();
        Generator.OnSendReportEvent += TakeReport;
        generators = GetComponentsInChildren<Spawner>();
        generatorCount = generators.Length;
        clearCount = 0;
        isWorked = false;
    }
   

    void CheckCount()
    {
        clearCount++;
        if (clearCount>=generatorCount)
        {
            Debug.Log(this+"클리어");
            SendReport();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && isWorked == false)
        {
            Debug.Log(this+"들어옴");
            isWorked = true;
            for (int i = 0; i < generators.Length; i++)
            {
                generators[i].StartGenerating();
            }
        }
    }


    protected override void TakeReport()
    {
        CheckCount();
    }
    protected override void SendReport()
    {
        base.SendReport();
        Debug.Log(this + "에서 SendReport");
    }
}
