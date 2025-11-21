using System;
using Core;
using Core.Interfaces;
using UnityEngine;

namespace Enemies
{
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth;
        [SerializeField] private float currentHealth;
        [SerializeField] private int moneyReward = 10;

        private float _currentHealth;
        
        private Action _onDeathCallback;
        public  Action OnDeath;
        private void OnEnable()
        {
            currentHealth = maxHealth;
        }

        public void Initialize(Action onDeathCallback)
        {
            _currentHealth = maxHealth;
            _onDeathCallback = onDeathCallback;
        }

        public void TakeDamage(float damage)
        {
            if (_currentHealth <= 0) return;
            
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            if (currentHealth <= 0)
                Die();
        }

        private void Die()
        {
            OnDeath?.Invoke();
            EventBus.Publish(new EnemyKilledEvent(moneyReward, transform.position));
            
            _onDeathCallback?.Invoke();
        }
      
    }
}