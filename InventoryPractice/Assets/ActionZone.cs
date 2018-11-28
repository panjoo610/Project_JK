using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionZone : MonoBehaviour {
    public GameObject ActiveGameObject;
    Collider collider;
    ParticleSystem[] particles;
    

	// Use this for initialization
	void Start () {
        collider = GetComponent<Collider>();
        particles = GetComponentsInChildren<ParticleSystem>();
	}
	
    void Interaction()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            Debug.Log("ASFD");
            var emission = particles[i].emission;
            emission.rateOverTime = 0; 
        }
        ActiveGameObject.SetActive(!ActiveGameObject.activeSelf);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("인");
            Interaction();
        }
    }


}
