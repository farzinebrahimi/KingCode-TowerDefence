using Core;
using Data;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Managers
{
    public class TowerPlacementManager : MonoBehaviour
    {
        [Header("TileMaps ")] [SerializeField] private Tilemap buildableTile;
        [SerializeField] private Tilemap pathTile;

        [Header("Tower Prefabs")] [SerializeField]
        private GameObject towerPrefab;
        
        [SerializeField]
        private TowerData _towerData;

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }
        
        private void OnEnable()
        {
            EventBus.Subscribe<MouseClickEvent>(OnMouseClicked);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<MouseClickEvent>(OnMouseClicked);
        }

        private void OnMouseClicked(MouseClickEvent e)
        {
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
            foreach (var lvl in _towerData.Levels)
            {   
                EventBus.Publish(new MoneyChangedEvent(lvl.upgradeCost));    
                
            }
            
            
        }
    }
}