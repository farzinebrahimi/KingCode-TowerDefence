using Core;
using Managers;
using TMPro;
using TowerSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TowerUpgradeUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject upgradePanel;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private TextMeshProUGUI upgradeCostText;
        [SerializeField] private TextMeshProUGUI currentLevelText;
        [SerializeField] private TextMeshProUGUI towerStatsText;

        private TowerUpgradeManager _upgradeManager;
        private Tower _selectedTower;

        private void Awake()
        {
            _upgradeManager = FindObjectOfType<TowerUpgradeManager>();
            
            upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
            
            upgradePanel.SetActive(false);
        }

        private void OnEnable()
        {
            EventBus.Subscribe<TowerSelectedEvent>(OnTowerSelected);
            EventBus.Subscribe<TowerDeselectedEvent>(OnTowerDeselected);
            EventBus.Subscribe<TowerUpgradedEvent>(OnTowerUpgraded);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<TowerSelectedEvent>(OnTowerSelected);
            EventBus.Unsubscribe<TowerDeselectedEvent>(OnTowerDeselected);
            EventBus.Unsubscribe<TowerUpgradedEvent>(OnTowerUpgraded);
        }

        private void OnTowerSelected(TowerSelectedEvent e)
        {
            _selectedTower = e.SelectedTower.GetComponent<Tower>();
            upgradePanel.SetActive(true);
            UpdateUI();
        }

        private void OnTowerDeselected(TowerDeselectedEvent e)
        {
            _selectedTower = null;
            upgradePanel.SetActive(false);
        }

        private void OnTowerUpgraded(TowerUpgradedEvent e)
        {
            UpdateUI();
        }

        private void OnUpgradeButtonClicked()
        {
            _upgradeManager.UpgradeSelectedTower();
        }

        private void UpdateUI()
        {
            if (_selectedTower == null) return;

            currentLevelText.text = $"{_selectedTower.GetCurrentLevel()}";

            towerStatsText.text = 
                $"Damage: {_selectedTower.GetDamage():F1}\n" +
                $"Range: {_selectedTower.GetRange():F1}\n" +
                $"FireRate: {_selectedTower.GetFireRate():F1}";

            bool canUpgrade = _selectedTower.CanUpgrade();
            upgradeButton.interactable = canUpgrade;

            if (canUpgrade)
            {
                int cost = _selectedTower.GetUpgradeCost();
                upgradeCostText.text = $"{cost}";
            }
            else
            {
                upgradeCostText.text = "Max Level!!!";
            }
        }
    }
}
