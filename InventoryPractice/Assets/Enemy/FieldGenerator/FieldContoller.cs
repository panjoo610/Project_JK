using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldContoller : AbstractMapController
{
    int SectorControllerCount;
    int clearCount;
    protected override void Start()
    {
        base.Start();
        SectorControllerCount = childrenMapContollers.Count;
        clearCount = 0;
    }

    void CheckCount()
    {
        clearCount++;
        if (clearCount >= SectorControllerCount)
        {
            Debug.Log(this + "클리어");
            SendReport();
        }
    }

    protected override void TakeReport()
    {
        CheckCount();
    }
    protected override void SendReport()
    {
        base.SendReport();
        Debug.Log("게임을 클리어함");
        //EnemyManager.instance.ClearStage();
    }

}
