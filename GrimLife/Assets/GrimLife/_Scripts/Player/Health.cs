using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private int maxHealth = 100;
    public Action OnDeath;
    public Action<int> OnDamage;
    public Image HealthBar;

    public void TakeDamage(int _damage)
    {
        if (transform.CompareTag("Player"))
            PlayerTakeDamage(_damage);

        if (transform.CompareTag("Enemy"))
            EnemyTakeDamage(_damage);

        health -= _damage;

        if (health <= 0)
        {
            OnDeath?.Invoke();
            Destroy(gameObject);
            return;
        }
        OnDamage?.Invoke(health / maxHealth);
    }
    public void SubscribeToDeath(Action _subscribee)
    {
        OnDeath += _subscribee;
    }
    public void UnSubscribeFromDeath(Action _subscribee)
    {
        OnDeath -= _subscribee;
    }
    private void Start()
    {
        health = maxHealth;
        if (transform.CompareTag("Enemy"))
            SubscribeToDeath(EnemyDied);
    }
    void PlayerTakeDamage(float _damage)
    {
        // set health ui to health - _damage
        if (health - _damage <= 0)
        {
            /// sfx player death sound
            SceneManager.LoadScene(0);
        }
        else
        {
            HealthBar.fillAmount = ((float)health / (float)maxHealth);
            // sfx player damage sound
        }
    }
    void EnemyTakeDamage(float _damage)
    {
        if (health - _damage <= 0)
        {
            // sfx enemy death sound
        }
        else
        {
            // sfx enemy damage sound
        }
    }
    void EnemyDied()
    {
        PlayerStats.Player.quest.Goal.EnemyKilled();
    }

    public void GiveHealth(int _health)
    {
        health += _health;
        if (health > maxHealth)
            health = maxHealth;
    }

    public float GetCurrentHealth() => health;

    public float GetMaxHealth() => maxHealth;
}

