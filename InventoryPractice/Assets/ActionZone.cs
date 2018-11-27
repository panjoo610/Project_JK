using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionZone : MonoBehaviour {
    Collider collider;
    public GameObject ActiveGameObject;

	// Use this for initialization
	void Start () {
        collider = GetComponent<Collider>();
	}
	



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ActiveGameObject.SetActive(!ActiveGameObject.activeSelf); 
        }
    }


}
