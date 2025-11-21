using Core.Interfaces;
using Data;
using Tower;

namespace Core
{
    public class StandardUpgradeStrategy : IUpgradeStrategy
    {
        public void ApplyUpgrade(TowerUpgradeSystem tower, TowerData.TowerLevel level)
        {
            tower.towerShooting.SetState(level.damage, level.fireRate);
            tower.towerTargeting.SetDetectionRadius(level.range);
        }
    }
}