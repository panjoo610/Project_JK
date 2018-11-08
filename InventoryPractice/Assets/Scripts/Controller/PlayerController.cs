using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    GameObject goalPlayer;

    FakePlayer fakePlayer;

    public Interactable Focus;

    Camera playerCamera;

    public LayerMask movementMask;

    public GameObject Muzzle, Bullet, BulletPosition;

    PlayerMotor motor;

    [SerializeField]
    Button fireButton;


    // Use this for initialization
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        fakePlayer = goalPlayer.GetComponent<FakePlayer>();
    }

    void Start ()
    {
        fireButton.onClick.AddListener(() => Fire());
        playerCamera = Camera.main;
        motor = GetComponent<PlayerMotor>();
        StageManager.instance.OnGameClearCallBack += RemoveFocus;
    }
	
//	// Update is called once per frame
//	void Update ()
//    {
//#if(UNITY_EDITOR)
//        InputAtEditor(); //에디터 인풋
//#endif

//#if (UNITY_ANDROID)
//        InputAtAndroid(); // 모바일 인풋
//#endif
//    }

    private void LateUpdate()
    {
        if (fakePlayer.IsMove)
        {
            Focus = null;
            motor.target = goalPlayer.transform;
            motor.MoveToPoint(goalPlayer.transform.position);
        }
        else
        {
            return;
        }       
    }


    //[Conditional("UNITY_EDITOR")] //에디터 내에서만 마우스 클릭
    void InputAtEditor()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { return; }
       
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null && fakePlayer.IsMove == false)
                {
                    SetFocus(interactable); //공격
                }
            }          
        }
    }

    //[Conditional("UNITY_ANDROID")] //안드로이드 모바일~
    void InputAtAndroid()
    {
        if (Input.touchCount > 0)
        {
            if (EventSystem.current.IsPointerOverGameObject(0) == false)
            {
                Vector2 pos = Input.GetTouch(0).position;

                Vector3 theTouch = new Vector3(pos.x, pos.y, 0.0f);

                if (Input.GetTouch(0).phase == TouchPhase.Stationary) //누르고 있음
                {
                    Ray ray = playerCamera.ScreenPointToRay(theTouch);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 100))
                    {
                        Interactable interactable = hit.collider.GetComponent<Interactable>();
                        if (interactable != null && fakePlayer.IsMove == false)
                        {
                            SetFocus(interactable); //공격
                        }
                    }
                }
            }
        }          
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            Interactable interactable = other.GetComponent<Interactable>();
            if (interactable != null)
            {
                SetFocus(interactable);
            }
        }
    }

    void Fire()
    {
        RaycastHit hit;

        Vector3 playerPosition = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);

        Debug.DrawRay(playerPosition, transform.forward * 10.0f);

        if(Physics.Raycast(playerPosition, transform.forward, out hit, 10.0f))
        {
            Debug.Log(hit.transform.name);
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null && fakePlayer.IsMove == false)
            {
                SetFocus(interactable); 
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
        Bullet.transform.position = BulletPosition.transform.position;       
    }

    IEnumerator ActiveMuzzle()
    {
        Muzzle.SetActive(!Muzzle.activeSelf);
        SoundManager.instance.PlaySFX("Fire", false);
        yield return new WaitForSeconds(0.2f);
        Muzzle.SetActive(!Muzzle.activeSelf);
    }    
}
