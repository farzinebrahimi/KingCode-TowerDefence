using Core;
using UnityEngine;
using UnityEngine.VFX;

namespace Enemies
{
    public class EnemyVfxHandler : MonoBehaviour
    {
        [SerializeField]
        private VisualEffect _deathVfx;
        private EnemyHealth _enemyHealth;
        
        

        private ObjectPool<EnemyVfxHandler> _pool;
        
        private void Awake()
        {
            _enemyHealth = GetComponent<EnemyHealth>();
        }

        private void OnEnable()
        {
           _enemyHealth.OnDeath +=  PlayDeathVfx;
           
        }

        private void OnDisable()
        {
            _enemyHealth.OnDeath -=  PlayDeathVfx;
        }

        private void PlayDeathVfx()
        {
            _deathVfx.Play();
        }
     
    }
}