using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private int maxHealth = 100;
    public Action OnDeath;
    public Action<int> OnDamage;

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
    public void GiveHealth(int _health)
    {
        health += _health;
        if (health > maxHealth)
            health = maxHealth;
    }
}

