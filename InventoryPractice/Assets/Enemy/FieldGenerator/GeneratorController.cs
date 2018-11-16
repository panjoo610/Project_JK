using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorController : SectorController
{
    [SerializeField]
    Generator[] generators;
    Generator Generator;
    int generatorCount;
    int clearCount;
    bool isWorked;

    public delegate void OnGenerateClear();
    public OnGenerateClear OnGenerateClearCallBack;

    // Use this for initialization
    protected override void Start() {
        //base.Start();
        Initialize();
    }
	void Initialize()
    {
        Generator = GetComponentInChildren<Generator>();
        Generator.OnGenerateOverCallBack += CheckCount;
        generators = GetComponentsInChildren<Generator>();
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
}
