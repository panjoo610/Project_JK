using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FakePlayer : MonoBehaviour
{

    public bool IsMove = false;

    Rigidbody PCrigidbody;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        InitTransform();

        PCrigidbody = GetComponent<Rigidbody>();
    }

    public void RunToPC(Vector3 moveVector)
    {
        PCrigidbody.velocity = moveVector * 250 * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(moveVector);
    }

    public void InitTransform()
    {
        transform.position = new Vector3(0.0f, 10.8f, -15.49f);
        transform.rotation = Quaternion.identity;
    }
}


