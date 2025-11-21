using System;
using Core.Interfaces;
using UnityEngine;

namespace Castle
{
    public class CastleHealth: MonoBehaviour , IDamageable
    {
        [SerializeField]
        private float _maxHealth;
        [SerializeField]
        private float _currentHealth;

        private void Start()
        {
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
            if (_currentHealth <= 0)
                Die();
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}