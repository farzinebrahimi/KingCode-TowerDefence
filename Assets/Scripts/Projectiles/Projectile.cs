using System;
using Core.Interfaces;
using UnityEngine;

namespace Projectiles
{
    public class Projectile : MonoBehaviour
    {
        private Rigidbody2D _rb;
        public Action OnReturnedToPool;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            if (_rb == null)
                Debug.LogError("Rigidbody2D not found!");
        }

        public void Launch(Vector2 direction, float speed)
        {
            _rb.linearVelocity = direction * speed;
            Invoke(nameof(ReturnToPool), 3f);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                IDamageable dmg = other.GetComponent<IDamageable>();
                if (dmg != null)
                {
                    dmg.TakeDamage(10.0f);
                }
                ReturnToPool();
            }
        }

        private void ReturnToPool()
        {
            _rb.linearVelocity = Vector2.zero;
            OnReturnedToPool?.Invoke();
        }
    }
}