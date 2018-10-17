using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (EventSystem.current.IsPointerOverGameObject()) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray,out hit, 100, movementMask))
            {
                motor.MoveToPoint(hit.point);

                RemoveFocus();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.Log(hit.collider.GetComponent<Interactable>());
                Interactable interactable =  hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    SetFocus(interactable);
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
            iTween.MoveTo(Bullet, iTween.Hash("position", focusPositon, "easeType", iTween.EaseType.easeInOutSine, "time", 0.2f));

            Invoke("HideBullet", 0.3f);
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
