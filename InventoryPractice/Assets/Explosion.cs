using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
    public ParticleSystem Particle;
    public SphereCollider[] colliders;
    public float ExplosionRange;

    public float DestroyTime;

    float runtime;
    // Use this for initialization
    void Start () {
        colliders = GetComponentsInChildren<SphereCollider>();
        test();
    }

    private void Update()
    {
        //particleSystem
        runtime += Time.deltaTime;
        if (runtime >= 5f)
        {
            test();
            DestroyTime -= Time.deltaTime;
            if (DestroyTime <= 0f)
            {
                gameObject.SetActive(false);
            } 
        }

    }

    void test()
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            //colliders[i].radius = Mathf.Lerp(colliders[i].radius, ExplosionRange, 10f * Time.deltaTime);
            //if (colliders[i].radius == ExplosionRange)
            //{
            //    colliders[i].enabled = false;
            //}
            colliders[i].enabled = true;
        }
    }

}
