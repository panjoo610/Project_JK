using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FakePlayer : MonoBehaviour
{

    public bool IsMove = false;

    Rigidbody PCrigidbody;

    [SerializeField]
    GameObject naviParticle;

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
        if (IsMove)
        {
            PCrigidbody.velocity = moveVector * 250 * Time.deltaTime;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(moveVector.x, 0f, moveVector.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
        else
        {
            return;
        }
    }

    public void ActiveSelfPartice(bool isActive)
    {
        naviParticle.SetActive(isActive);
    }

    public void InitTransform()
    {
        transform.position = new Vector3(0f, 10.86f, -14.607f);
        transform.rotation = Quaternion.identity;
    }
}


