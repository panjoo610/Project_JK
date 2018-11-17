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
        EnemyManager.instance.ClearStage();
    }

}
