using UnityEngine;

public class CharacterStats : MonoBehaviour {

    public int maxHealth = 100;
    public int currentHealth { get; protected set; }


    public Stats damage;
    public Stats armor;

    public event System.Action<int, int> OnHealthChanged;
    

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);
        currentHealth -= damage;
        //Debug.Log(transform.name + "takes " + damage + " damage.");

        if(OnHealthChanged != null)
        {
            OnHealthChanged(maxHealth, currentHealth);
        }

        if(currentHealth <= 0)
        {
            Die();        
        }
    }

    public virtual void Die()
    {
        //Debug.Log(transform.name + "died.");
    }

    public virtual void Initialization()
    {
        maxHealth = 100;
        currentHealth = maxHealth;

        if (OnHealthChanged != null)
        {
            OnHealthChanged(maxHealth, currentHealth);
        }
    }
}
