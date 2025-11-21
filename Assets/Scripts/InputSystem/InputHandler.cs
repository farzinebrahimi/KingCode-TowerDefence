using System;
using Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputHandler : MonoBehaviour
    {
        private Camera _camera;
        private InputActions _inputActions; 
        
        private bool _clickPending;
        private Vector2 _clickScreenPos;

        

        private void Awake()
        {
            _camera = Camera.main;
            _inputActions = new InputActions(); 
        }

        private void OnEnable()
        {
            _inputActions.Enable();
            _inputActions.Gameplay.Click.performed += OnClickPerformed;
            _inputActions.Gameplay.Release.performed += OnClickRelease;
        }

        private void OnDisable()
        {
            _inputActions.Disable();
            _inputActions.Gameplay.Click.performed -= OnClickPerformed;
            _inputActions.Gameplay.Release.performed -= OnClickRelease;
        }

        private void OnClickRelease(InputAction.CallbackContext ctx)
        {
            
        }

        private void OnClickPerformed(InputAction.CallbackContext ctx)
        {
            _clickScreenPos = Mouse.current.position.ReadValue();
            _clickPending = true;
        }

        private void LateUpdate()
        {
            if (!_clickPending) return;
            _clickPending = false;

            if (EventSystem.current.IsPointerOverGameObject())
                return;
            

            Vector3 worldPos = _camera.ScreenToWorldPoint(_clickScreenPos);
            worldPos.z = 0f;

            EventBus.Publish(new MouseClickEvent(worldPos));
        }
    }
}