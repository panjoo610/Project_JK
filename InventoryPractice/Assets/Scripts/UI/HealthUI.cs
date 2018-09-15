﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterStats))]
public class HealthUI : MonoBehaviour {


    public GameObject UIPrefab;
    public Transform target;
    float visibleTime = 5f;

    float lastMadeVisibleTime;

    Transform ui;
    Image healthSlider;
    Transform cam;

	// Use this for initialization
	void Start ()
    {
        cam = Camera.main.transform;

		foreach(Canvas c in FindObjectsOfType<Canvas>())
        {
            if(c.renderMode == RenderMode.WorldSpace)
            {
                ui = Instantiate(UIPrefab, c.transform).transform;
                healthSlider = ui.GetChild(0).GetComponent<Image>();
                ui.gameObject.SetActive(false);
                break;
            }
        }
        GetComponent<CharacterStats>().OnHealthChanged += OnHealthChanged;

    }
	void OnHealthChanged(int maxHealth, int currentHeath)
    {
        if(ui != null)
        {
            ui.gameObject.SetActive(true);
            lastMadeVisibleTime = Time.time;

            float healthPercent = (float)currentHeath / maxHealth;
            healthSlider.fillAmount = healthPercent;
            if (currentHeath <= 0)
            {
                Destroy(ui.gameObject);
            }
        }
    }
    // Update is called once per frame
    void LateUpdate ()
    {
        if(ui != null)
        {
            ui.position = target.position;
            ui.forward = new Vector3(-cam.position.x, 0, -cam.position.z);

            if (Time.time - lastMadeVisibleTime > visibleTime)
            {
                ui.gameObject.SetActive(false);
            }
        }
	}
}
