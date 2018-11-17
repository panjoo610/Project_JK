using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    Transform VirtualTransform;
    [SerializeField]
    Vector3 VirtualPosition;
    float maxDistance = 2f;
    bool IsMoving;

    [SerializeField]
    GameObject naviParticle;

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
        //fakePlayer = goalPlayer.GetComponent<FakePlayer>();
        //VirtualRigidbody = VirtualPlayer.GetComponent<Rigidbody>();
        VirtualPosition = gameObject.transform.position;
    }

    void Start ()
    {
        fireButton.onClick.AddListener(() => OnFire());
        playerCamera = Camera.main;
        motor = GetComponent<PlayerMotor>();
        StageManager.instance.OnGameClearCallBack += RemoveFocus;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(VirtualPosition, 1f);
    }
    private void LateUpdate()
    {
        VirtualTransform.position = VirtualPosition;
        if (IsMoving)
        {
            Focus = null;
            //motor.target = goalPlayer.transform;
            //motor.MoveToPoint(goalPlayer.transform.position);
            //motor.target = VirtualPlayer.transform.position;
            //motor.MoveToPoint(VirtualPlayer.transform.position);
            motor.target = VirtualTransform;
            motor.MoveToPoint(VirtualPosition);
            
        }
        else
        {
            Focus = null;
            return;
        }
        
    }
    
    public void MoveVirtualPosition(Vector3 moveVector, bool isMove)
    {
        IsMoving = isMove;
        if (isMove)
        {
            float distance = Vector3.Distance(VirtualPosition, gameObject.transform.position);
            if (distance <= maxDistance)
            {
                motor.agent.isStopped=false;
                VirtualPosition += moveVector * motor.agent.speed * Time.deltaTime;
            }
            else if (maxDistance < motor.agent.remainingDistance)
            {
                VirtualPosition = Vector3.Lerp(VirtualPosition, gameObject.transform.position + (moveVector * 0.9f), motor.agent.speed * Time.deltaTime);
                motor.MoveToPoint(VirtualPosition);
                motor.agent.isStopped = true;
            }
            else
            {
                VirtualPosition = Vector3.Lerp(VirtualPosition, gameObject.transform.position + (moveVector * 0.9f), motor.agent.speed * Time.deltaTime);
                motor.MoveToPoint(VirtualPosition);
                motor.agent.isStopped = false;
            }
            naviParticle.SetActive(isMove);
        }
        else
        {
            VirtualPosition = Vector3.Lerp(VirtualPosition, gameObject.transform.position+(moveVector*0.9f), motor.agent.speed/ (motor.agent.speed+1) * Time.deltaTime);
            naviParticle.SetActive(isMove);
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

    void OnFire()
    {
        RaycastHit hit;

        Vector3 playerPosition = new Vector3(transform.position.x, transform.position.y + 1.7f, transform.position.z);

        Debug.DrawRay(playerPosition, transform.forward * 20.0f);

        if(Physics.Raycast(playerPosition, transform.forward, out hit, 20.0f))
        {
            //Debug.Log(hit.transform.name);
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null && IsMoving == false)
            {
                if (interactable.tag == "Enemy")
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

    //[Conditional("UNITY_EDITOR")] //에디터 내에서만 마우스 클릭
    //void InputAtEditor()
    //{
    //    if (EventSystem.current.IsPointerOverGameObject()) { return; }

    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;

    //        if (Physics.Raycast(ray, out hit, 100))
    //        {
    //            Interactable interactable = hit.collider.GetComponent<Interactable>();
    //            if (interactable != null && fakePlayer.IsMove == false)
    //            {
    //                if (interactable.tag == "Monster")
    //                    SetFocus(interactable); //공격
    //            }
    //        }
    //    }
    //}

    //[Conditional("UNITY_ANDROID")] //안드로이드 모바일~
    //void InputAtAndroid()
    //{
    //    if (Input.touchCount > 0)
    //    {
    //        if (EventSystem.current.IsPointerOverGameObject(0) == false)
    //        {
    //            Vector2 pos = Input.GetTouch(0).position;

    //            Vector3 theTouch = new Vector3(pos.x, pos.y, 0.0f);

    //            if (Input.GetTouch(0).phase == TouchPhase.Stationary) //누르고 있음
    //            {
    //                Ray ray = playerCamera.ScreenPointToRay(theTouch);
    //                RaycastHit hit;

    //                if (Physics.Raycast(ray, out hit, 100))
    //                {
    //                    Interactable interactable = hit.collider.GetComponent<Interactable>();
    //                    if (interactable != null && fakePlayer.IsMove == false)
    //                    {
    //                        if (interactable.tag == "Monster")
    //                            SetFocus(interactable); //공격
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

}
