using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraContorller : MonoBehaviour {
    public Transform target;
    public Vector3 shakePos;
    
    public Vector3 offset;

    public Image HitImage;
    public Color HitImageColor;

    private float currentZoom = 10.0f;
    private float currentYaw = 0f;

    public float yawSpeed = 100f;

    public float pitch = 2.0f;

    public float zoomSpeed = 4f;
    public float minZoom = 5f;
    public float maxZoom = 15f;

    bool isShake;
    
    private void Start()
    {
        HitImageColor = HitImage.color;
    }

    public void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        currentYaw -= Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime;
        
    }

    private void LateUpdate()
    {
        
        transform.position = target.position + shakePos - offset * currentZoom;
        //transform.position = transform.position + shakePos.position;
        transform.LookAt(target.position + Vector3.up * pitch);

        transform.RotateAround(target.position, Vector3.up, currentYaw);
    }

    public void ShakeCamera()
    {
        if (!isShake)
        {
            isShake = true;
            StartCoroutine(Shake(.25f, .7f)); 
        }
        
    }
    IEnumerator Shake(float duation, float magnitude)
    {
        Vector3 originalPos = shakePos;

        float elapsed = 0.0f;
        HitImageColor.a = 0f;
        HitImage.color = HitImageColor;
        while (elapsed < duation)
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
