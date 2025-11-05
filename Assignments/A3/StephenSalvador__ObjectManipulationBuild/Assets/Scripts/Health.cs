using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] private float currentHealth;
    [SerializeField] private float minHealth = 0f;
    [SerializeField] private float maxHealth = 100f;

    public event Action<GameObject> OnDamaged;
    public event Action OnDied;
    public event Action<GameObject> OnHealed;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void ApplyDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth < minHealth)
        {
            currentHealth = minHealth;
        }

        // Trigger OnDamaged event (no origin provided)
        OnDamaged?.Invoke(null);

        if (currentHealth <= minHealth)
        {
            OnDied?.Invoke();
            gameObject.SetActive(false);
        }
    }

    public void ApplyDamage(float damage, GameObject origin)
    {
        currentHealth -= damage;

        if (currentHealth < minHealth)
        {
            currentHealth = minHealth;
        }

        // Trigger event for external listeners (e.g. LootDrop, AI)
        OnDamaged?.Invoke(origin);

        if (currentHealth <= minHealth)
        {
            OnDied?.Invoke();
            gameObject.SetActive(false);
        }
    }

    public void HealDamage(float amount, GameObject origin)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        OnHealed?.Invoke(origin);
    }
}