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
        InputAtEditor(); //에디터 인풋

        InputAtAndroid(); // 모바일 인풋
    }
    
    [Conditional("UNITY_EDITOR")]
    void InputAtEditor()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { return; }

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

        // 일단 분리함 모바일에서 어떻게 해결할지 생각해볼 것
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
    }

    [Conditional("UNITY_ANDROID")]
    void InputAtAndroid()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { return; }

        if (Input.touchCount > 0)
        {
            Vector2 pos = Input.GetTouch(0).position;

            Vector3 theTouch = new Vector3(pos.x, pos.y, 0.0f);  

            if (Input.touchCount == 1)   
            {

                Ray ray = playerCamera.ScreenPointToRay(theTouch);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100, movementMask))
                {
                    motor.MoveToPoint(hit.point);//이동

                    RemoveFocus();
                }
            }

            if (Input.touchCount >= 2)   
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
