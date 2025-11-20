using System;
using Core;
using Core.Interfaces;
using Data;
using Managers;
using UnityEngine;

namespace Tower
{
    public class TowerUpgradeSystem : MonoBehaviour
    {
        [Header("References")]
        public TowerData towerData;
       
        [Header("Components")]
         public TowerShooting towerShooting;
         public TowerTargeting towerTargeting;
        
        [Header("Current State")]
        [SerializeField] private int currentLevel = 0;
        [SerializeField] private bool canUpgrade = true;
        
        private CurrencyManager _currencyManager;

        [SerializeField] private ScriptableObject upgradeStrategyAsset;
        private IUpgradeStrategy _upgradeStrategy;
        
        private void Awake()
        {
            _currencyManager = FindObjectOfType<CurrencyManager>();
            _upgradeStrategy = upgradeStrategyAsset as IUpgradeStrategy;
            
            if (towerShooting == null)
                towerShooting = GetComponent<TowerShooting>();
                
            if (towerTargeting == null)
                towerTargeting = GetComponentInChildren<TowerTargeting>();
        }
        
        private void Start()
        {
            if (towerData.Levels.Count == 0)
            {
                towerData.Levels.Add(new TowerData.TowerLevel
                {
                    level = 1,
                    damage = 10,
                    range = 5,
                    fireRate = 1,
                    upgradeCost = 0
                });
            }
    
            
            ApplyLevelStats();
        }

        private void OnEnable()
        {
            EventBus.Subscribe<MoneyChangedEvent>(OnMoneyChanged);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<MoneyChangedEvent>(OnMoneyChanged);
        }

        private void OnMoneyChanged(MoneyChangedEvent e)
        {
            ApplyLevelStats();
        }
        
        private void ApplyLevelStats()
        {
            if (towerData.Levels.Count == 0 || currentLevel < 0 || currentLevel >= towerData.Levels.Count)
                return;

            var currentLevelData = towerData.Levels[currentLevel];
            towerShooting?.SetState(currentLevelData.damage, currentLevelData.fireRate);
            towerTargeting?.SetDetectionRadius(currentLevelData.range);

            canUpgrade = currentLevel < towerData.Levels.Count - 1 &&
                         _currencyManager.GetCurrentMoney() >= towerData.Levels[currentLevel + 1].upgradeCost;
        }

        public bool UpgradeTower()
        {
            int nextLevelIndex = currentLevel + 1;
            
            if (currentLevel >= towerData.Levels.Count - 1)
            {
                return false;
            }

            int upgradeCost = towerData.Levels[currentLevel + 1].upgradeCost;

            ISpendMoney spendMoney = GetComponent<ISpendMoney>();
            if(spendMoney != null)
                spendMoney.SpendMoney(upgradeCost);
            currentLevel++;
            ApplyLevelStats();
            
            
            EventBus.Publish(new TowerUpgradedEvent(gameObject, currentLevel));
            
            return true;
        }

      
        
        public int GetCurrentLevel() => currentLevel;
        public TowerData.TowerLevel GetCurrentLevelData() => towerData.Levels[currentLevel];
        public bool CanUpgrade() => canUpgrade;
        public int GetNextUpgradeCost() => currentLevel < towerData.Levels.Count - 1 
            ? towerData.Levels[currentLevel + 1].upgradeCost 
            : -1;
    }
}