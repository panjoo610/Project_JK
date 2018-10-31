using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FakePlayer : MonoBehaviour
{
    Rigidbody PCrigidbody;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        InitTransform();
        //StageManager.instance.OnMoveLobbySceneCallBack += InitTransform;
        PCrigidbody = GetComponent<Rigidbody>();
    }
    public void RunToPC(Vector3 moveVector)
    {
        PCrigidbody.velocity = moveVector * 150 * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(moveVector);
    }
    public void InitTransform()
    {
        transform.position = new Vector3(0.0f, 10.8f, -15.49f);
        //각도도 초기화할 것
    }
}


