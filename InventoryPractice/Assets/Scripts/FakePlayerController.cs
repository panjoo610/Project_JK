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
    Vector3 moveVector = Vector3.zero;

    float h, v;

    Vector3 moverDir;

    void FixedUpdate()
    {
        moveVector = PoolInput();

        if(moveVector != Vector3.zero)
        {
            player.IsMove = true;
            player.RunToPC(PoolInput());
            OnJoystick(moveVector);
        }
        else
        {
            OnJoystick(moveVector);
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

    void OnJoystick(Vector3 inputVector)
    {          
        if(inputVector != Vector3.zero)
        {
            if (inputVector.x < 0 && inputVector.z < 0)
            {
                Virtualonclick.joyStick[1].gameObject.SetActive(false);
                Virtualonclick.joyStick[2].gameObject.SetActive(false);
                Virtualonclick.joyStick[3].gameObject.SetActive(false);

                Virtualonclick.joyStick[0].gameObject.SetActive(true);
            }
            else if (inputVector.x < 0 && inputVector.z > 0)
            {
                Virtualonclick.joyStick[0].gameObject.SetActive(false);
                Virtualonclick.joyStick[2].gameObject.SetActive(false);
                Virtualonclick.joyStick[3].gameObject.SetActive(false);

                Virtualonclick.joyStick[1].gameObject.SetActive(true);
            }
            else if (inputVector.x > 0 && inputVector.z < 0)
            {
                Virtualonclick.joyStick[0].gameObject.SetActive(false);
                Virtualonclick.joyStick[1].gameObject.SetActive(false);
                Virtualonclick.joyStick[3].gameObject.SetActive(false);

                Virtualonclick.joyStick[2].gameObject.SetActive(true);
            }
            else if (inputVector.x > 0 && inputVector.z > 0)
            {
                Virtualonclick.joyStick[0].gameObject.SetActive(false);
                Virtualonclick.joyStick[1].gameObject.SetActive(false);
                Virtualonclick.joyStick[2].gameObject.SetActive(false);

                Virtualonclick.joyStick[3].gameObject.SetActive(true);
            }
        }
        else
        {
            Virtualonclick.joyStick[0].gameObject.SetActive(false);
            Virtualonclick.joyStick[1].gameObject.SetActive(false);
            Virtualonclick.joyStick[2].gameObject.SetActive(false);
            Virtualonclick.joyStick[3].gameObject.SetActive(false);
        }
        //0은 11시, 1은 1시, 
        //2는 7시, 3은 5시
    }
}