using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int baseHealth = 100;
    [SerializeField] private int currentHealth;
    private int maxHealth;

    public event Action<DeathType> OnDie;
    public event Action<int> OnHealthChanged;

    void Start()
    {
        maxHealth = baseHealth;
        currentHealth = maxHealth;
    }

    public void AddToMaxHealth(int amountToAdd)
    {
        maxHealth = baseHealth + amountToAdd;
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, DeathType deathType)
    {
        OnHealthChanged?.Invoke(currentHealth);

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die(deathType);
        }
    }

    public void Die(DeathType deathType)
    {
        OnDie?.Invoke(deathType);
    }
}
