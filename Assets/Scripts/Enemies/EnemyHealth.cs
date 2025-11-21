using Core;
using Core.Interfaces;
using Managers;
using UnityEngine;

namespace Enemies
{
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth;
        [SerializeField] private float currentHealth;
        [SerializeField] private int moneyReward = 10;

        private void Start()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            if (currentHealth <= 0)
                Die();
        }

        private void Die()
        {
            Destroy(gameObject,0.1f);
            EventBus.Publish(new EnemyKilledEvent(moneyReward, transform.position));
        }
    }
}