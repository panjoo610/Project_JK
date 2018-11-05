using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterStats))]
public class LayerHealthBar : MonoBehaviour {
    
    public GameObject HealthBarPrefab;
    public float Speed;
    public float VisibleTime;
    float visibleRunTime;

    [Header("HealthBar Color"),Tooltip("0번이 가장 마지막에 나옴, 나머지는 순차(베이스컬러 제외 2개이상 추천)")]
    public Color[] colors;
    Color[] usingColors;

    EnemyStats enemyStats;
    int healthBarCount;
    float healthValue;

    /// <summary>
    /// backSlider가 상위 오브젝트 위치
    /// </summary>
    struct HealthBar
    {
        public Image BackSlider;
        public Image FrontSlider;
    }
    Queue<HealthBar> queue;
    HealthBar ActiveBar;

    Transform baseTransfrom;
    Transform activeBarTransform;

    private void Start()
    {
        InitializeHealthBarCount();
        SetColorOrder();
        queue = new Queue<HealthBar>();
        InstantiateHealthBar(queue);
        ActiveBar = queue.Dequeue();
    }
    void InstantiateHealthBar(Queue<HealthBar> queue)
    {
        foreach (Canvas c in FindObjectsOfType<Canvas>())
        {
            if (c.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                baseTransfrom = Instantiate(HealthBarPrefab, c.transform).transform;
                //ActiveBar.BackSlider.transform.SetParent(c.transform);
                //healthSlider = ui.GetChild(0).GetComponent<Image>();
                //ui.gameObject.SetActive(false);
                for (int i = 0; i < 2; i++)
                {
                    HealthBar healthBar = new HealthBar();
                    healthBar.BackSlider = HealthBarPrefab.transform.GetChild(i).GetComponent<Image>();
                    healthBar.FrontSlider = healthBar.BackSlider.transform.GetChild(0).GetComponent<Image>();
                    ResetHealthBar(healthBar, healthBarCount);
                    queue.Enqueue(healthBar);
                }
                break;
            }
        }
    }

    void LateUpdate()
    {
        //if (ui != null)
        //{
        //    ui.position = target.position;
        //    if (Time.time - visibleRunTime > VisibleTime)
        //    {
        //        ui.gameObject.SetActive(false);
        //    }
        //}
    }
    void TestOnHealthChanged(int currntHealth)
    {
        int TakeDamage = enemyStats.maxHealth - currntHealth;
        if (TakeDamage > healthValue)
        {
            float OverDamage = TakeDamage - healthValue;
            float currntDamage = TakeDamage - OverDamage;
        }

        float healthPercent = (healthValue - TakeDamage) / healthValue;
        Debug.Log("Health Percent : "+healthPercent);
        if (healthPercent >= 0)
        {
            ActiveBar.FrontSlider.fillAmount = healthPercent;
            StartCoroutine(HealthSliderChange(healthPercent, ActiveBar.BackSlider));
        }
        else if(healthPercent < 0)
        {
            ActiveBar.FrontSlider.fillAmount = 0;
            StartCoroutine(HealthSliderChange(0, ActiveBar.BackSlider));
            SwichingActiveBar();

        }
        //currntHealth

    }
    void OnHealthChanged(int maxHealth, int currntHealth)
    {
        float healthPercent = (float)currntHealth / maxHealth;
        ActiveBar.FrontSlider.fillAmount = healthPercent;
        StartCoroutine(HealthSliderChange(healthPercent, ActiveBar.BackSlider));
    }

    IEnumerator HealthSliderChange(float healthPercent, Image slider)
    {
        while (slider.fillAmount >= healthPercent)
        {
            slider.fillAmount = Mathf.Lerp(slider.fillAmount, healthPercent, Speed * Time.deltaTime);
            yield return new WaitForSeconds(0.2f);
        }
        yield return null;
    }
    /// <summary>
    /// 활성화 HealthBar 교체(Enqueue, Transform)
    /// </summary>
    void SwichingActiveBar()
    {
        ActiveBar.BackSlider.transform.SetAsLastSibling();
        ResetHealthBar(ActiveBar, healthBarCount);
        queue.Enqueue(ActiveBar);

        ActiveBar = queue.Dequeue();
        ActiveBar.BackSlider.transform.SetAsFirstSibling();
    }

    void ResetHealthBar(HealthBar healthBar, int colorIndex)
    {
        healthBarCount--;
        healthBar.FrontSlider.fillAmount = 1f;
        healthBar.BackSlider.fillAmount = 1f;
        
        //여기서 색상 변경 - 순서대로 색상 지정하게 수정해야함
        healthBar.FrontSlider.color = usingColors[colorIndex];
        Color backColoer = new Color(usingColors[colorIndex].r - 0.1f, usingColors[colorIndex].g - 0.1f, usingColors[colorIndex].b - 0.1f);
        healthBar.BackSlider.color = backColoer;
    }

    void InitializeHealthBarCount()
    {
        enemyStats = GetComponent<EnemyStats>();
        healthBarCount = enemyStats.maxHealth / 100;
        healthValue = enemyStats.maxHealth / healthBarCount;
    }

    void SetColorOrder()
    {
        usingColors = new Color[healthBarCount];
        usingColors[0] = colors[0];
        int j = 1;

        for (int i = 1; i < healthBarCount; i++)
        {
            if (colors[j] != null)
            {
                usingColors[i] = colors[j];
                j++;
            }
            else
            {
                j = 1;
                usingColors[i] = colors[j];
                j++;
            }
        }
    }







    //가장 마지막으로 순서 변경
    //Transform.SetAsLastSibling
    //Move the transform to the end of the local transfrom list.

    //가장 처음으로 순서 변경
    //Transform.SetAsFirstSibling
    //Move the transform to the start of the local transfrom list.

    //순서 설정(index 값)
    //Transform.SetSiblingIndex
    //Sets the sibling index.

    //현재 순서 반환(index 값)
    //Transform.GetSiblingIndex
    //Gets the sibling index.
}
