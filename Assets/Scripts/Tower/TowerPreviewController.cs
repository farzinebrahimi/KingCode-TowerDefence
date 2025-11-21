using Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tower
{
    public class TowerPreviewController : MonoBehaviour
    {
        private Camera _cam;
        private bool _isFollowing = false;

        [SerializeField] private GameObject spritePrefab;
        
        private GameObject _currentSprite;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _cam = Camera.main;
            
            InitializePreview();
        }

        private void InitializePreview()
        {
            _currentSprite = Instantiate(spritePrefab);
            _spriteRenderer = _currentSprite.GetComponent<SpriteRenderer>();
            _spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            _currentSprite.SetActive(false);
        }

        private void OnEnable()
        {
            if (_cam == null)
                _cam = Camera.main;

            EventBus.Subscribe<BeginTowerPlacementEvent>(OnShowPreviewHandle);
            EventBus.Subscribe<TowerPlacedEvent>(OnTowerPlaced);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<BeginTowerPlacementEvent>(OnShowPreviewHandle);
            EventBus.Unsubscribe<TowerPlacedEvent>(OnTowerPlaced);
        }

        private void OnShowPreviewHandle(BeginTowerPlacementEvent e)
        {
            if (!_isFollowing)
            {
                ShowPreview();
            }
            else
            {
                HidePreview();
            }
        }

        private void ShowPreview()
        {
            if (_currentSprite == null)
            {
                InitializePreview();
            }
            
            _currentSprite.SetActive(true);
            _isFollowing = true;
            EventBus.Publish(new TowerPlacementStateChangedEvent(true));
        }

        private void HidePreview()
        {
            if (_currentSprite != null)
            {
                _currentSprite.SetActive(false);
            }
            
            _isFollowing = false;
            EventBus.Publish(new TowerPlacementStateChangedEvent(false));
        }

        private void OnTowerPlaced(TowerPlacedEvent e)
        {
            HidePreview();
        }

        private void Update()
        {
            if (!_isFollowing || _currentSprite == null) return;

            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector3 worldPos = _cam.ScreenToWorldPoint(mousePos);
            worldPos.z = 0f;
            _currentSprite.transform.position = worldPos;
        }

        private void OnDestroy()
        {
            if (_currentSprite != null)
            {
                Destroy(_currentSprite);
            }
        }
    }
}
