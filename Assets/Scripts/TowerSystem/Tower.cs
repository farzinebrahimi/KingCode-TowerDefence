using Data;
using UnityEngine;

namespace TowerSystem
{
    public class Tower : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private TowerData towerData;
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        private TowerShooting _shooting;
        private TowerTargeting _targeting;

        [Header("Current state")] 
        [SerializeField] private int currentLevel = 0;

        private float _damage;
        private float _range;
        private float _fireRate;

        private void Awake()
        {
            _shooting = GetComponent<TowerShooting>();
            _targeting = GetComponent<TowerTargeting>();
        }

        private void Start()
        {

            ApplyLevelState();
        }

        public void Upgrade()
        {
            if (!CanUpgrade())
            {
                return;
            }

            currentLevel++;
            ApplyLevelState();
            
        }

        private void ApplyLevelState()
        {
            if (currentLevel >= towerData.Levels.Count)
            {
                return;
            }
                
            var levelData = towerData.Levels[currentLevel];
            
            _damage = levelData.damage;
            _range = levelData.range;
            _fireRate = levelData.fireRate;
            
            UpdateComponents();
            
        }

        private void UpdateComponents()
        {
            if (_shooting != null)
            {
                _shooting.SetState(_damage, _fireRate);
            }

            if (_targeting != null)
            {
                _targeting.SetDetectionRadius(_range);
            }
        }

        public int GetCurrentLevel() => currentLevel;
        public int GetUpgradeCost() => towerData.GetUpgradeCost(currentLevel);
        public bool CanUpgrade() => towerData.CanUpgrade(currentLevel);
        public float GetDamage() => _damage;
        public float GetRange() => _range;
        public float GetFireRate() => _fireRate;
        public TowerData GetTowerData() => towerData;
    }
}
