using System;
using Core;
using TowerSystem;
using UnityEngine;

namespace Managers
{
    public class TowerSelectionManager : MonoBehaviour
    {
        [Header("Selection Settings")]
        [SerializeField] private LayerMask towerLayer;
        
        [Header("Visual Feedback")]
        [SerializeField] private GameObject selectionIndicatorPrefab;
        
        private Transform _currentSelectedTower;
        private GameObject _selectionIndicator;
        
        
        private bool _isPlacementActive;

        private void OnEnable()
        {
            EventBus.Subscribe<MouseClickEvent>(OnMouseClicked);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<MouseClickEvent>(OnMouseClicked);
        }

        private void OnPlacementStateChanged(TowerPlacementStateChangedEvent e)
        {
            _isPlacementActive = e.IsPlacementActive;
            
            if (_isPlacementActive && _currentSelectedTower != null)
            {
                DeselectTower();
            }
        }

        private void OnMouseClicked(MouseClickEvent e)
        {

            if (_isPlacementActive)
            {
                return;
            }

            if (e.IsConsumed)
            {
                return;
            }

            Vector2 mouseWorldPos = e.WorldPosition;
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 0f, towerLayer);

            if (hit.collider != null)
            {
                Transform clickedTransform = hit.collider.transform;
        
                Tower towerComponent = clickedTransform.GetComponentInParent<Tower>();
        
                if (towerComponent != null)
                {
                    Transform towerTransform = towerComponent.transform;
                    SelectTower(towerTransform);
                    e.Consume();
                }
                else
                {
                    Debug.Log($"[Selection] No Tower component found on {clickedTransform.name} or its parents");
                }
            }
            else
            {
                DeselectTower();
            }
        }

        private void SelectTower(Transform tower)
        {
            if (_currentSelectedTower == tower)
            {
                DeselectTower();
                return;
            }

            if (_currentSelectedTower != null)
            {
                DeselectTower();
            }

            _currentSelectedTower = tower;
            
            ShowSelectionIndicator();
            
            EventBus.Publish(new TowerSelectedEvent(_currentSelectedTower));
            
        }

        private void DeselectTower()
        {
            if (_currentSelectedTower == null) return;

            Transform previousTower = _currentSelectedTower;
            _currentSelectedTower = null;
            
            HideSelectionIndicator();
            
            EventBus.Publish(new TowerDeselectedEvent(previousTower));
            
        }

        private void ShowSelectionIndicator()
        {
            if (selectionIndicatorPrefab == null) return;

            if (_selectionIndicator == null)
            {
                _selectionIndicator = Instantiate(selectionIndicatorPrefab);
            }

            _selectionIndicator.transform.position = _currentSelectedTower.position;
            _selectionIndicator.transform.SetParent(_currentSelectedTower);
            _selectionIndicator.SetActive(true);
        }

        private void HideSelectionIndicator()
        {
            if (_selectionIndicator != null)
            {
                _selectionIndicator.SetActive(false);
            }
        }
    }
}
