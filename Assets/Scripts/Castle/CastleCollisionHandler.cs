using System;
using Core.Interfaces;
using UnityEngine;

namespace Towers
{
    public class CastleCollisionHandler : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(10.0f);
            }
        }
    }
}