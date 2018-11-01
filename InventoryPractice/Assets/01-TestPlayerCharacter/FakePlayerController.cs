using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakePlayerController : MonoBehaviour
{
    [SerializeField]
    FakePlayer player;

    [SerializeField]

    IOnclick Virtualonclick;
    [SerializeField]
    Vector3 moveVector;

    float h, v;

    Vector3 moverDir;

    void Start()
    {
        moveVector = Vector3.zero;
    }

    void FixedUpdate()
    {
        moveVector = PoolInput();

        if(moveVector != Vector3.zero)
        {
            player.IsMove = true;
            player.RunToPC(PoolInput());
        }
        else
        {
            player.IsMove = false;
        }
    }

    Vector3 PoolInput()
    {
        h = Virtualonclick.Horizontal();
        v = Virtualonclick.Vertical();

        moverDir = new Vector3(-v, 0, h).normalized;

        return moverDir;
    }
}