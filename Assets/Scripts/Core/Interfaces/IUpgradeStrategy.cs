using Data;
using Tower;

namespace Core.Interfaces
{
    public interface IUpgradeStrategy
    {
        void ApplyUpgrade(TowerUpgradeSystem tower, TowerData.TowerLevel level);
    }
}