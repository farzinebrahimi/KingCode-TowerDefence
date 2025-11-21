using System;
using Core;
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
        private Camera _cam;

        private void OnEnable()
        {
            EventBus.Subscribe<MouseClickEvent>(OnMouseClicked);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<MouseClickEvent>(OnMouseClicked);
        }

        private void Awake()
        {
            _cam = Camera.main;
        }

        private void OnMouseClicked(MouseClickEvent e)
        {
            Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, towerLayer);

            if (hit.collider != null)
            {
                SelectTower(hit.transform);
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
