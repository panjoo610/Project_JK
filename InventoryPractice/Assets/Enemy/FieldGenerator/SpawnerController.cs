using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : AbstractMapController
{
    [SerializeField]
    Spawner[] generators;
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
        generators = GetComponentsInChildren<Spawner>();
        generatorCount = childrenMapContollers.Count;
        clearCount = 0;
        isWorked = false;
    }
   

    void CheckCount()
    {
        clearCount++;
        if (clearCount>=generatorCount)
        {
            SendReport();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && isWorked == false)
        {
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
    }
}
