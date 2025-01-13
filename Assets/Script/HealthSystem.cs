using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int health=100;

    public event EventHandler onDead;
    public event EventHandler onDmg;
    private int healthMax;
    private void Awake()
    {
        healthMax = health;
    }
    public void Damage(int dmgAmount)
    {
        health -= dmgAmount; 
        if (health < 0) health = 0;
        onDmg?.Invoke(this, EventArgs.Empty);
        if (health == 0)
        {
            Die();
        }

        Debug.Log($"Health: {health}");
    }

    private void Die()
    {
        onDead?.Invoke(this, EventArgs.Empty);
    }
    public float GetHealthNormalized()
    {
        return (float)health / healthMax;
    }
}
