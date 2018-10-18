using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterStats))]
public class PlayerHealthUI : MonoBehaviour
{

    [SerializeField]
    Image healthSlider;

    // Use this for initialization
    void Start()
    {
        GetComponent<CharacterStats>().OnHealthChanged += OnHealthChanged;
        healthSlider.fillAmount = 1;
    }
    void OnHealthChanged(int maxHealth, int currentHeath)
    {
        float healthPercent = (float)currentHeath / maxHealth;
        healthSlider.fillAmount = healthPercent;
    }
}

