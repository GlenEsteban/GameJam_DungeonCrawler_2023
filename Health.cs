using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] bool isPlayer;
    [SerializeField] float health = 100f;
    float startingHealth;
    HealthUI healthUI;
    void Start() {
        startingHealth = health;
        healthUI = FindObjectOfType<HealthUI>();
    }

    public void TakeDamage(int damage) {
        health = Mathf.Max(0, health - damage);
        UpdateHealthBar();

        // Check for death
        if (health == 0) {
        }     
    }

    public void RecoverHealth(int healAmount) {
        health += healAmount;
        if (health > startingHealth) {
            health = startingHealth;
        }
        UpdateHealthBar();
    }

    void UpdateHealthBar() {
        float currentFill = health / startingHealth;
        if (isPlayer) {
            healthUI.UpdateHealthBar("player", currentFill);
        }
        else {
            healthUI.UpdateHealthBar("enemy", currentFill);
        }
    }
}
