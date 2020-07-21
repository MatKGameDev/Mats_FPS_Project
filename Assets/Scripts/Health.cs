﻿using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public UnityAction<float> onDamaged;
    public UnityAction onDie;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float a_damageAmount)
    {
        currentHealth -= a_damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (onDamaged != null)
            onDamaged.Invoke(a_damageAmount);

        if (currentHealth <= 0f && onDie != null)
            onDie.Invoke();
    }
}
