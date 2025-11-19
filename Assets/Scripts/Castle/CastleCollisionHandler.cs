using Core.Interfaces;
using UnityEngine;

namespace Castle
{
    public class CastleCollisionHandler : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {

            if (other.CompareTag("Enemy"))
            {
                IDamageable selfDmg = GetComponent<IDamageable>();
                if (selfDmg != null)
                {
                    selfDmg.TakeDamage(50.0f);
                } 
                IDamageable damageable = other.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(999f);
                }
            }
        }
    }
}