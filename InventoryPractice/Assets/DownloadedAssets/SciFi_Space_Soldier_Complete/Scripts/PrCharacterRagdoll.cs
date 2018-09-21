using UnityEngine;
using System.Collections;

public class PrCharacterRagdoll : MonoBehaviour {

    public GameObject[] ragdollBones;
    public bool DeactivateAtStart = true;

    private ParticleSystem[] VFXParticles;
     
	// Use this for initialization
	void Start () {
        InitializeRagdoll();
        if (GetComponentInChildren<ParticleSystem>())
            VFXParticles = GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
	void Update () {
	
	}

    public void InitializeRagdoll()
    {
        Rigidbody[] temp = transform.GetComponentsInChildren<Rigidbody>();
        ragdollBones = new GameObject[temp.Length];
        int t = 0;
        foreach (Rigidbody r in temp)
        {
            if (r.gameObject.name != gameObject.name)
            {
                ragdollBones.SetValue(r.gameObject, t);
                t += 1;
            }
        }

        if (DeactivateAtStart)
        {
            foreach (GameObject GO in ragdollBones)
            {
                if (GO != null)
                {
                    GO.GetComponent<Collider>().enabled = false;
                    GO.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
    }

    public void ActivateRagdoll()
    {
        if (GetComponent<Animator>())
            GetComponent<Animator>().enabled = false;
        foreach (GameObject GO in ragdollBones)
        {
            if (GO != null)
            {
                GO.GetComponent<Collider>().enabled = true;
                GO.GetComponent<Rigidbody>().isKinematic = false;
                
            }
        }
        
    }
    public void SetForceToRagdoll(Vector3 position, Vector3 force)
    {
        ActivateRagdoll();

        //getClosestBone
        float dist = 200f;
        GameObject targetBone = ragdollBones[0];
        foreach (GameObject go in ragdollBones)
        {
            if (go != null)
            {
                float tempDist = Vector3.Distance(go.transform.position, position);
                if (dist > tempDist)
                {
                    dist = tempDist;
                    targetBone = go;
                }
            }
               
        }
        //Debug.Log(ragdollBones[6].name + " force " + force + " pos " + position);
        targetBone.GetComponent<Rigidbody>().AddForceAtPosition(force, position,ForceMode.Impulse);
    }

    public void SetExplosiveForce(Vector3 position)
    {
        Debug.Log("Explosion Acitvated");
        InitializeRagdoll();
        //ActivateRagdoll();

        foreach (GameObject go in ragdollBones)
        {
            if (go != null)
            {
                //go.GetComponent<Rigidbody>().AddForceAtPosition(position * 25f, position, ForceMode.Impulse);
                go.GetComponent<Rigidbody>().AddExplosionForce(20.0f, position, 2.0f, 0.25f, ForceMode.Impulse);
                Debug.Log(go.name);
            }
        }

        if (VFXParticles.Length > 0)
        {
            foreach (ParticleSystem p in VFXParticles)
            {
                p.Play();
            }
        }
    }
}
