using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGeneratorController : MonoBehaviour {
    [SerializeField]
    TestGenerator[] generators;

    
    // Use this for initialization
    void Start () {
        Initialize();
    }
	void Initialize()
    {
        generators = GetComponentsInChildren<TestGenerator>();
    }
   


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            for (int i = 0; i < generators.Length; i++)
            {
                generators[i].StartGenerating();
            }
        }
    }
}
