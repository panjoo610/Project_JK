using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorController : AbstractMapController
{
    public GameObject WallObject;
    int GeneratorControllerCount;
    int clearCount;
    protected override void Start()
    {
        base.Start();
        GeneratorControllerCount = childrenMapContollers.Count;
        clearCount = 0;

        WallObject = transform.Find(WallObject.name).gameObject;
    }

    void CheckCount()
    {
        clearCount++;
        if (clearCount >= GeneratorControllerCount)
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
        OpenTheWay();
    }

    void OpenTheWay()
    {
        WallObject.SetActive(!WallObject.activeSelf);
    }


}
