using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour {
    public int EnemyCount;
    public Transform[] GeneratorTransforms;
    public GameObject EnemyPrefab;

    GameObject[] EnemyObjects;
	// Use this for initialization
	void Start () {
        EnemyObjects = new GameObject[EnemyCount];
        for (int i = 0; i < EnemyObjects.Length; i++)
        {
            EnemyObjects[i] = Instantiate(EnemyPrefab, transform, false);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
