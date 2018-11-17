using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMapController : MonoBehaviour {
    
    [SerializeField]
    protected AbstractMapController childMapContoller;

    [SerializeField]
    protected List<AbstractMapController> childrenMapContollers;

    public delegate void OnSendReport();
    public OnSendReport OnSendReportEvent;

    protected virtual void Start()
    {
        Initialize();
    }
    void Initialize()
    {
        childrenMapContollers = FindChildeComponents();
        for (int i = 0; i < childrenMapContollers.Count; i++)
        {
            childrenMapContollers[i].OnSendReportEvent += TakeReport;
        }
    }
    protected List<AbstractMapController> FindChildeComponents()
    {
        List<AbstractMapController>  childMapContollers = new List<AbstractMapController>();
        
        for (int i = 0; i < transform.childCount; i++)
        {
            AbstractMapController s = transform.GetChild(i).GetComponent<AbstractMapController>();
            if (s != null)
            {
                childMapContollers.Add(s);
            }
        }
        return childMapContollers;
    }

    //protected AbstractMapController FindChildeComponent()
    //{
    //    childMapContoller = transform.GetChild(0).GetComponent<AbstractMapController>();
    //    return childMapContoller;
    //}
    
    protected abstract void TakeReport();
    protected virtual void SendReport()
    {
        if (OnSendReportEvent != null)
            OnSendReportEvent.Invoke();
    }
}
