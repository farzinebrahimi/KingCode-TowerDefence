using System.Linq;
using Core;
using Core.Interfaces;
using TowerSystem;
using UnityEngine;

namespace Managers
{
    public class TowerUpgradeManager : MonoBehaviour
    {
        private ICurrencyManager _currencyManager;
        private Tower _selectedTower;

        private void Awake()
        {
            _currencyManager = CurrencyManager.Instance;
        }

        private void OnEnable()
        {
            EventBus.Subscribe<TowerSelectedEvent>(OnTowerSelected);
            EventBus.Subscribe<TowerDeselectedEvent>(OnTowerDeselected);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<TowerSelectedEvent>(OnTowerSelected);
            EventBus.Unsubscribe<TowerDeselectedEvent>(OnTowerDeselected);
        }

        private void OnTowerSelected(TowerSelectedEvent e)
        {
            Debug.Log($"[UpgradeManager] Tower selected: {e.SelectedTower?.name ?? "NULL"}");
    
            if (e.SelectedTower == null)
            {
                Debug.LogError("[UpgradeManager] SelectedTower is NULL in event!");
                return;
            }
    
            _selectedTower = e.SelectedTower.GetComponent<Tower>();

            if (_selectedTower == null)
            {
                Debug.LogError($"[UpgradeManager] Tower component not found on {e.SelectedTower.name}!");
                Debug.Log($"[UpgradeManager] Available components: {string.Join(", ", e.SelectedTower.GetComponents<Component>().Select(c => c.GetType().Name))}");
                return;
            }
    
            Debug.Log($"[UpgradeManager] Tower component found! Level: {_selectedTower.GetCurrentLevel()}");
        }

        private void OnTowerDeselected(TowerDeselectedEvent e)
        {
            _selectedTower = null;
        }
        
        public void UpgradeSelectedTower()
        {
            if (_selectedTower == null)
            {
                Debug.LogWarning("Any tower not available to upgrade!");
                return;
            }

            if (!_selectedTower.CanUpgrade())
            {
                Debug.Log("Tower's level is max!!!");
                EventBus.Publish(new TowerUpgradeFailedEvent(_selectedTower, "Max Level"));
                return;
            }

            int upgradeCost = _selectedTower.GetUpgradeCost();

            if (!_currencyManager.SpendMoney(upgradeCost))
            {
                EventBus.Publish(new TowerUpgradeFailedEvent(_selectedTower, "Money is not enough!"));
                return;
            }

            _selectedTower.Upgrade();
            
            EventBus.Publish(new TowerUpgradedEvent(_selectedTower));
        }
        
        public bool CanSelectedTowerUpgrade()
        {
            if (_selectedTower == null) return false;
            
            return _selectedTower.CanUpgrade() && 
                   _currencyManager.HasMoney(_selectedTower.GetUpgradeCost());
        }
        
        public int GetSelectedTowerUpgradeCost()
        {
            return _selectedTower != null ? _selectedTower.GetUpgradeCost() : 0;
        }

        
        public int GetSelectedTowerLevel()
        {
            return _selectedTower != null ? _selectedTower.GetCurrentLevel() : 0;
        }
    }
}
