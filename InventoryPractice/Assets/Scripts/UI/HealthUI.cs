using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterStats))]
public class HealthUI : MonoBehaviour {


    public GameObject UIPrefab;
    public Transform target;
    float visibleTime = 5f;
    
    float lastMadeVisibleTime;
    EnemyStats enemyStats;
    Transform ui;
    Image healthSlider;
    Transform cam;

	// Use this for initialization
	void Start ()
    {
        foreach (Canvas c in FindObjectsOfType<Canvas>())
        {
            if (c.renderMode == RenderMode.WorldSpace)
            {
                ui = Instantiate(UIPrefab, c.transform).transform;
                healthSlider = ui.GetChild(0).GetComponent<Image>();
                ui.gameObject.SetActive(true);
                break;
            }
        }
        target = gameObject.transform;
        //ui = Instantiate(UIPrefab, gameObject.transform).transform;
        //healthSlider = ui.GetChild(0).GetComponent<Image>();
        //ui.gameObject.SetActive(false);
        enemyStats = GetComponent<EnemyStats>();
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
                ui.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (ui != null)
        {
            ui.position = target.position;
        }
    }

    private void OnEnable()
    {
        if (ui != null && healthSlider.fillAmount < 1f)
        {
            healthSlider.fillAmount = 1f;
            ui.gameObject.SetActive(true); 
        }
    }

    private void OnDestroy()
    {
        if (ui != null)
        {
            Destroy(ui.gameObject); 
        }
    }

}
