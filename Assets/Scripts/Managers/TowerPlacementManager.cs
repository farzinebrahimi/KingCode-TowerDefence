using Core;
using Core.Interfaces;
using Data;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Managers
{
    public class TowerPlacementManager : MonoBehaviour
    {
        [Header("TileMaps ")] [SerializeField] private Tilemap buildableTile;
        [SerializeField] private Tilemap pathTile;

        [Header("Tower Prefabs")] 
        [SerializeField]
        private GameObject towerPrefab;
        
        [Header("References")]
        [SerializeField]
        private TowerData currentTowerData;
        [SerializeField]
        private PlayerData playerData;
        private int _currentTowerUpgradeCost;
        
        private ISpendMoney _currencyManager;


        
        private  bool _canPlaceTower = false;
      

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            _currencyManager = CurrencyManager.Instance;
        }

        private void OnEnable()
        {
            EventBus.Subscribe<MouseClickEvent>(OnMouseClicked);
            EventBus.Subscribe<TowerPlacementStateChangedEvent>(OnPlacementStateChanged);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<MouseClickEvent>(OnMouseClicked);
            EventBus.Unsubscribe<TowerPlacementStateChangedEvent>(OnPlacementStateChanged);
        }

        private void OnPlacementStateChanged(TowerPlacementStateChangedEvent e)
        {
            _canPlaceTower = e.IsPlacementActive;
        }
        

        private void OnMouseClicked(MouseClickEvent e)
        {
            if (!_canPlaceTower) return;

            int buildCost = currentTowerData.Levels[0].buildCost;
            
            foreach (var towerLevel in currentTowerData.Levels)
            {
                _currentTowerUpgradeCost = towerLevel.upgradeCost;
            }

            if (!_currencyManager.SpendMoney(buildCost))
                return;
            
            Vector3 mousePosition = e.WorldPosition; 
            Vector3Int tilePosition = buildableTile.WorldToCell(mousePosition);

            bool isBuildable = buildableTile.GetTile(tilePosition) != null;
            bool isPath = pathTile.GetTile(tilePosition) != null;
            

            if (!isBuildable || isPath)
            {
                Debug.LogWarning("Cannot place tower here");
                return;
            }

            Vector3 cellCenter = buildableTile.GetCellCenterWorld(tilePosition);
            Collider2D overlap = Physics2D.OverlapPoint(cellCenter, LayerMask.GetMask("TowerBase"));

            if (overlap != null)
            {
                Debug.LogWarning("Tower already placed here");
                return;
            }

            GameObject tower = Instantiate(towerPrefab, cellCenter, Quaternion.identity);
            EventBus.Publish(new TowerPlacedEvent(tower.transform));

            _canPlaceTower = false;

        }
    }
}