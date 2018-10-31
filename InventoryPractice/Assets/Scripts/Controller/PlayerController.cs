using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {


    public Interactable Focus;

    Camera playerCamera;

    public LayerMask movementMask;

    public GameObject Muzzle, Bullet, BulletPoint;

    PlayerMotor motor;

    // Use this for initialization
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start ()
    {
        playerCamera = Camera.main;
        motor = GetComponent<PlayerMotor>();
        StageManager.instance.OnGameClearCallBack += RemoveFocus;
    }
	
	// Update is called once per frame
	void Update ()
    {
#if(UNITY_EDITOR)
        InputAtEditor(); //에디터 인풋
#endif

#if (UNITY_ANDROID)
        InputAtAndroid(); // 모바일 인풋
#endif
    }

    [Conditional("UNITY_EDITOR")] //에디터 내에서만 마우스 클릭
    void InputAtEditor()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { return; }
       
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    SetFocus(interactable); //공격
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, movementMask))
            {
                motor.MoveToPoint(hit.point);//이동

                RemoveFocus();
            }
        }


    }

    [Conditional("UNITY_ANDROID")] //안드로이드 모바일~
    void InputAtAndroid()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (EventSystem.current.IsPointerOverGameObject(i) == false)
                {
                    Vector2 pos = Input.GetTouch(i).position;

                    Vector3 theTouch = new Vector3(pos.x, pos.y, 0.0f);

                    if (Input.GetTouch(i).phase == TouchPhase.Stationary) //누르고 있음
                    {
                        Ray ray = playerCamera.ScreenPointToRay(theTouch);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, 100))
                        {
                            Interactable interactable = hit.collider.GetComponent<Interactable>();
                            if (interactable != null)
                            {
                                SetFocus(interactable); //공격
                            }
                        }
                    }

                    if (Input.GetTouch(i).phase == TouchPhase.Began) //터치
                    {
                        Ray ray = playerCamera.ScreenPointToRay(theTouch);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, 100, movementMask))
                        {
                            motor.MoveToPoint(hit.point);//이동

                            RemoveFocus();
                        }
                    }
                }            
            }
        }          
    }

    void SetFocus(Interactable newFocus)
    {
        if(newFocus != Focus)
        {
            if(Focus!=null)
            Focus.OnDeFocused();

            Focus = newFocus;
            motor.FollowTarget(newFocus);
        }
      
        newFocus.OnFocused(transform);     
    }

    public void RemoveFocus()
    {
        if(Focus != null)
        Focus.OnDeFocused();

        Focus = null;
        motor.StopFollowTarget();
    }

    public void AttackHit_AnimationEvent()
    {
        StartCoroutine(ActiveMuzzle());
        
        if(Focus != null)
        {
            Bullet.SetActive(true);

            Vector3 focusPositon = new Vector3(Focus.transform.position.x, Focus.transform.position.y + 1.0f, Focus.transform.position.z);
            iTween.MoveTo(Bullet, iTween.Hash("position", focusPositon, "easeType", iTween.EaseType.easeInOutSine, "time", 0.1f));
            //거리에 비례해서 총알이 점점 사라지게 만들 것
            Invoke("HideBullet", 0.15f);
        }
    }

    public void HideBullet()
    {
        Bullet.SetActive(false);
        Bullet.transform.position = BulletPoint.transform.position;       
    }

    IEnumerator ActiveMuzzle()
    {
        Muzzle.SetActive(!Muzzle.activeSelf);
        SoundManager.instance.PlaySFX("Fire", false);
        yield return new WaitForSeconds(0.2f);
        Muzzle.SetActive(!Muzzle.activeSelf);
    }    
}
