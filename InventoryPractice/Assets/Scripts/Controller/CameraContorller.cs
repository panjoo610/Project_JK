using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraContorller : MonoBehaviour
{
    public Transform target;
    public Vector3 shakePos;
    
    public Vector3 offset;

    public Image HitImage;
    public Color HitImageColor;

    [SerializeField]
    public float currentZoom = 10.0f;
    float baseZoom;
    [SerializeField]
    private float currentYaw = 0f;

    public float yawSpeed = 100f;

    public float pitch = 2.0f;

    public float zoomSpeed = 4f;
    public float minZoom = 5f;
    public float maxZoom = 15f;

    bool isShake;
    IEnumerator shakeCameraEnumerator;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {   
        target = PlayerManager.instance.Player.transform;
        shakeCameraEnumerator = Shake(.25f, .7f);
        HitImageColor = HitImage.color;
        baseZoom = currentZoom;
    }

    private void LateUpdate()
    {        
        transform.position = target.position + shakePos - offset * currentZoom;

        transform.LookAt(target.position + Vector3.up * pitch);

        transform.RotateAround(target.position, Vector3.up, currentYaw);
    }

    public void HideHitImage()
    {
        HitImage.gameObject.SetActive(false);
        StopCoroutine(shakeCameraEnumerator);
        shakeCameraEnumerator = Shake(.25f, .7f);
    }

    public void ShakeCamera()
    {
        HitImage.gameObject.SetActive(true);
        if (!isShake)
        {
            isShake = true;
            StartCoroutine(shakeCameraEnumerator);
        }
        shakeCameraEnumerator = Shake(.25f, .7f);
    }

    public void RobbyCamera()
    {
        offset = new Vector3(-0.48f, -0.28f, -0.58f); //로비씬
        currentZoom = 5f;
    }


    public void CameraZoomInOut(float ZoomValue)
    {
        float newZoomValue = baseZoom * (1+ ZoomValue);
        currentZoom = newZoomValue;
    }

    IEnumerator DirectingCameraCoroutine(float x, float y, float z)
    {
        Vector3 goalPosition = new Vector3(x, y, z);

        float duration = 3.0f;

        float elapsed = .0f;

        float zoom = 5.0f;

        Vector3 prvePositoin = offset;

        while (elapsed < duration)
        {
            offset = Vector3.Lerp(prvePositoin, goalPosition, elapsed / duration);
            
            currentZoom = zoom * elapsed / duration;
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        currentZoom = 5f;    
    }

    public void ActingCombat()
    {
        StartCoroutine(DirectingCameraCoroutine(-1f, -2.0f, 0f));
    }

    IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = shakePos;

        float elapsed = 0.0f;
        HitImageColor.a = 0f;
        HitImage.color = HitImageColor;
        while (elapsed < duration)
        {
            if (HitImageColor.a <= 128)
            {
                HitImageColor.a += 12 * Time.deltaTime; 
            }
            HitImage.color = HitImageColor;
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            shakePos = new Vector3(x, y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        shakePos = originalPos;
        while (HitImageColor.a>0)
        {
            HitImageColor.a -= 5 * Time.deltaTime;
            HitImage.color = HitImageColor;
            yield return null;
        }
        isShake = false;
    }
}
