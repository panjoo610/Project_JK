using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{

    #region Singleton
    public static StageManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
