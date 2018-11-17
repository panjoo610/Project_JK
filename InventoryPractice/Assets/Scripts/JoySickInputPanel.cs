using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoySickInputPanel : MonoBehaviour
{
    [SerializeField]
    //FakePlayer player;
    PlayerController playerController;

    [SerializeField]
    IOnclick Virtualonclick;

    [SerializeField]
    Vector3 moveVector = Vector3.zero;

    float h, v;

    Vector3 moverDir;

    StageManager stageManager;

    public bool isGameState;

    private void Start()
    {
        isGameState = false;

        stageManager = StageManager.instance;

        stageManager.OnGameClearCallBack += TurnOffJoystick;

        stageManager.OnGameOverCallBack += TurnOffJoystick;

        stageManager.OnGameStartCallBack += TurnOnJoystick;
        playerController = PlayerManager.instance.playerController;
    }

    void FixedUpdate()
    {
        if (playerController == null)
        {
            playerController = PlayerManager.instance.playerController;
        }
        if (isGameState)
        {
            moveVector = PoolInput();
        }
        else
        {
            moveVector = Vector3.zero;
        }

        if(moveVector != Vector3.zero && PlayerManager.instance.playerStats.characterCombat.IsAttack == false)
        {
            //player.IsMove = true;
            //player.RunToPC(PoolInput());
            playerController.MoveVirtualPosition(PoolInput(), true);
            //playerController.MoveVirtualPlayer(PoolInput(), true);
            OnJoystick(moveVector);
            //player.ActiveSelfPartice(true);
        }
        else
        {
            //player.IsMove = false;
            //player.ActiveSelfPartice(false);
            //playerController.MoveVirtualPlayer(PoolInput(), false);
            playerController.MoveVirtualPosition(PoolInput(), false);
            OnJoystick(moveVector);        
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
            if (inputVector.x < 0 && inputVector.z < 0)  {ActiveJoystick(0); } // 0은 11시 방향

            else if (inputVector.x < 0 && inputVector.z > 0) {ActiveJoystick(1);} // 1은 1시 방향

            else if (inputVector.x > 0 && inputVector.z < 0) {ActiveJoystick(2);} // 2는 7시 방향

            else if (inputVector.x > 0 && inputVector.z > 0) {ActiveJoystick(3);} //3은 5시 방향
        }
        else { ActiveJoystick(-1); }
    }

    void ActiveJoystick(int ActiveNum)
    {
        if(ActiveNum < 0)
        {
            for (int i = 0; i < 4; i++)
            {
                Virtualonclick.joyStick[i].gameObject.SetActive(false);
            }
            return;
        }

        for (int i = 0; i < 4; i++)
        {
            if(ActiveNum == i)
            {
                Virtualonclick.joyStick[i].gameObject.SetActive(true);
            }
            else
            {
                Virtualonclick.joyStick[i].gameObject.SetActive(false);
            }
        }
    }
    void TurnOffJoystick()
    {
        isGameState = false;
    }
    void TurnOnJoystick()
    {
        isGameState = true;
    }
}