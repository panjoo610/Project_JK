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
    float nowHealth;
    [Header("HealthBar Color"),Tooltip("0번이 가장 마지막에 나옴, 나머지는 순차(베이스컬러 제외 2개이상 추천)")]
    public Color[] colors;
    Color[] usingColors;

    EnemyStats enemyStats;

    [SerializeField]
    int healthBarCount;
    float healthValue;

    Image backGroundImage;
    /// <summary>
    /// backSlider가 상위 오브젝트 위치
    /// </summary>
    private struct HealthBar
    {
        public Image BackSlider;
        public Image FrontSlider;
        public float HealthPoint;

    }
    Queue<HealthBar> queue;
    HealthBar ActiveBar;
    Transform baseTransfrom;


    bool IsChangeing;

    private void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        nowHealth = enemyStats.maxHealth;
        InitializeHealthBarCount();
        SetColorOrder();
        queue = new Queue<HealthBar>();
        InstantiateHealthBar(queue);
        ActiveBar = queue.Dequeue();
        baseTransfrom.gameObject.SetActive(false);
        GetComponent<CharacterStats>().OnHealthChanged += OnHealthChanged;
        StageManager.instance.OnGameClearCallBack += HideHealthBar;
        StageManager.instance.OnGameOverCallBack += HideHealthBar;
        StageManager.instance.OnMoveLobbySceneCallBack += HideHealthBar;
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
                backGroundImage = baseTransfrom.GetChild(0).GetComponent<Image>();
                for (int i = 1; i <= 2; i++)
                {
                    HealthBar healthBar = new HealthBar();
                    healthBar.BackSlider = baseTransfrom.GetChild(i).GetComponent<Image>(); //HealthBarPrefab.transform
                    healthBar.FrontSlider = healthBar.BackSlider.transform.GetChild(0).GetComponent<Image>();
                    healthBar.HealthPoint = healthValue;
                    ResetHealthBar(healthBar, healthBarCount);
                    healthBar.BackSlider.transform.SetAsFirstSibling();
                    queue.Enqueue(healthBar);
                }
                backGroundImage.transform.SetAsFirstSibling();
                break;
            }
        }
    }

    void LateUpdate()
    {
        if (baseTransfrom != null)
        {
            //baseTransfrom.position = target.position;
            if (Time.time - visibleRunTime > VisibleTime && IsChangeing == false)
            {
                baseTransfrom.gameObject.SetActive(false);
            }
        }
    }
    
    void OnHealthChanged(int maxHealth, int currntHealth)
    {
        if (healthBarCount>=0)
        {
            baseTransfrom.gameObject.SetActive(true);
            visibleRunTime = Time.time;

            if (nowHealth <= 0)
            {
                baseTransfrom.gameObject.SetActive(false);
            }

            float TakeDamage = nowHealth - currntHealth;
            nowHealth -= TakeDamage;

            if (ActiveBar.HealthPoint > TakeDamage)
            {
                float healthPercent = ((ActiveBar.HealthPoint - TakeDamage) / healthValue);
                ActiveBar.HealthPoint -= TakeDamage;
                ActiveBar.FrontSlider.fillAmount = healthPercent;
                StartCoroutine(HealthSliderChange(healthPercent, ActiveBar, false, 0));
            }
            else
            {
                float OverDamage = TakeDamage - ActiveBar.HealthPoint;
                Debug.Log("OverDamage" + OverDamage);
                ActiveBar.HealthPoint = 0;
                ActiveBar.FrontSlider.fillAmount = 0;
                StartCoroutine(HealthSliderChange(0, ActiveBar, true, OverDamage));
                SwichingActiveBar(ActiveBar);
                nowHealth += OverDamage;
            } 
        }
    }

    IEnumerator HealthSliderChange(float healthPercent, HealthBar healthBar ,bool DamageIsOver, float OverDamage)
    {
        IsChangeing = true;
        while (healthBar.BackSlider.fillAmount >= healthPercent)
        {
            healthBar.BackSlider.fillAmount = Mathf.Lerp(healthBar.BackSlider.fillAmount, healthPercent, Speed * Time.deltaTime);
            if (healthBar.BackSlider.fillAmount <= 0.005)
            {
                Debug.Log(healthBarCount);
                HealthBarSorting(healthBar, healthBarCount);
                break;
            }
            yield return null;
        }
        yield return null;
        if (DamageIsOver == true)
        {
            OnHealthChanged(1, (int)(nowHealth - OverDamage));
        }
        IsChangeing = false;
    }
    /// <summary>
    /// 활성화 HealthBar 교체(Enqueue, Transform)
    /// </summary>
    void SwichingActiveBar(HealthBar healthBar)
    {
        ActiveBar = queue.Dequeue();
    }

    void HealthBarSorting(HealthBar healthBar, int colorIndex)
    {
        queue.Enqueue(ResetHealthBar(healthBar, colorIndex));
        healthBar.BackSlider.transform.SetAsLastSibling();
        ActiveBar.BackSlider.transform.SetAsLastSibling();
        backGroundImage.transform.SetAsFirstSibling();
    }

    HealthBar ResetHealthBar(HealthBar healthBar, int colorIndex)
    {
        colorIndex--;

        healthBar.FrontSlider.fillAmount = 1f;
        healthBar.BackSlider.fillAmount = 1f;
        healthBar.HealthPoint = healthValue;

        if (colorIndex >=0)
        {
            healthBar.FrontSlider.color = usingColors[colorIndex];
            Color backColoer = new Color(usingColors[colorIndex].r - 0.1f, usingColors[colorIndex].g - 0.1f, usingColors[colorIndex].b - 0.1f, usingColors[colorIndex].a);
            healthBar.BackSlider.color = backColoer;

            if (colorIndex > 1)
            {
                backGroundImage.color = usingColors[colorIndex - 1];
            }
            else
            {
                backGroundImage.color = Color.black;
            }
            healthBarCount--;
            Debug.Log(healthBarCount);
            if (healthBarCount<0)
            {
                Debug.Log(healthBarCount + " BossIsDead");
                //baseTransfrom.gameObject.SetActive(false);
            }
        }
        else
        {
            healthBar.BackSlider.gameObject.SetActive(false);
        }
        return healthBar;
    }

    /// <summary>
    /// HealthBar의 HealthPoint 정하기
    /// </summary>
    void InitializeHealthBarCount()
    {
        enemyStats = GetComponent<EnemyStats>();
        healthBarCount = enemyStats.maxHealth / 100;
        healthValue = enemyStats.maxHealth / healthBarCount;
    }

    /// <summary>
    /// 색상배열생성
    /// </summary>
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
    void HideHealthBar()
    {
        //baseTransfrom.gameObject.SetActive(false);
        Destroy(baseTransfrom.gameObject);
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
