using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "TowerData", menuName = "Data/TowerData")]
    public class TowerData : ScriptableObject
    {
        [Header("Tower Info")] public string towerName;
        public string description;

        [Header("Levels")] public List<TowerLevel> Levels = new();

        [Header("Growth Settings")] public float damageGrowthMultiplier = 1.2f;

        public float rangeGrowthMultiplier = 1.15f;

        public float fireRateGrowthMultiplier = 1.1f;

        public float upgradeCostGrowthMultiplier = 1.5f;

        public int GetUpgradeCost(int currentLevel)
        {
            if (currentLevel >= Levels.Count)
                return 0;
            return Levels[currentLevel + 1].upgradeCost;
        }

        public bool CanUpgrade(int currentLevel)
        {
            return currentLevel < Levels.Count - 1;
        }


        [ContextMenu("Generate Next Levels")]
        public TowerLevel GenerateNextLevel()
        {
            if (Levels.Count == 0)
                throw new InvalidOperationException("No base level defined for generation!");

            var lastLevel = Levels[^1];

            var newLevel = new TowerLevel
            {
                level = lastLevel.level + 1,
                damage = lastLevel.damage * damageGrowthMultiplier,
                range = lastLevel.range * rangeGrowthMultiplier,
                fireRate = lastLevel.fireRate * fireRateGrowthMultiplier,
                buildCost =  0,
                upgradeCost = Mathf.RoundToInt(
                    (lastLevel.upgradeCost == 0 ? 10 : lastLevel.upgradeCost)
                    * upgradeCostGrowthMultiplier)
            };

            Levels.Add(newLevel);
            return newLevel;
        }
        [ContextMenu("Generate 5 Levels")]
        public void Generate5Levels()
        {
            for (int i = 0; i < 5; i++)
            {
                GenerateNextLevel();
            }
        }


        [Serializable]
        public class TowerLevel
        {
            [Header("Combat Stats")] 
            public float damage;
            public float range;
            public float fireRate;

            [Header("Build & Upgrade")]
             public int buildCost;
            public int upgradeCost;
            public int level;
            
        }
    }
}