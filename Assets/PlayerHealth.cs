using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void PlayerTakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Player took damage: " + amount + " | Current Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            PlayerDie();
        }
    }

    void PlayerDie()
    {
        Debug.Log("Player died!");
        //Zaimplementowac smierc gracza 
    }
}
