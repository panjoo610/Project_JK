using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMapController : MonoBehaviour {
    
    protected AbstractMapController childMapContoller;

    [SerializeField]
    protected AbstractMapController[] childMapContollers;


    protected virtual void Start()
    {
        Initialize();
    }
    void Initialize()
    {
        childMapContollers = FindChildeComponents();
    }
    protected AbstractMapController FindChildeComponent()
    {
        childMapContoller = transform.GetChild(0).GetComponent<AbstractMapController>();
        return childMapContoller;
    }
    
    protected AbstractMapController[] FindChildeComponents()
    {
        int i = 0;
        while (transform.GetChild(i) == null)
        {
            i++;
            childMapContollers[i] = transform.GetChild(i).GetComponent<AbstractMapController>();
        }
        
        return childMapContollers;
    }



    protected virtual void SendReport()
    {

    }

    protected virtual void TakeReport()
    {

    }
}
