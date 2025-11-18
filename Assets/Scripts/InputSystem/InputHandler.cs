using System;
using Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputHandler : MonoBehaviour
    {
        private Camera _camera;
        private InputActions _inputActions; 

        public Vector2 MouseScreenPosition => Mouse.current.position.ReadValue();

        private void Awake()
        {
            _camera = Camera.main;
            _inputActions = new InputActions(); 
        }

        private void OnEnable()
        {
            _inputActions.Enable();
            _inputActions.Gameplay.Click.performed += OnClickPerformed;
        }

        private void OnDisable()
        {
            _inputActions.Gameplay.Click.performed -= OnClickPerformed;
            _inputActions.Disable();
        }

        private void OnClickPerformed(InputAction.CallbackContext ctx)
        {
           

            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector3 worldPos = _camera.ScreenToWorldPoint(screenPos);
            worldPos.z = 0f;

            EventBus.Publish(new MouseClickEvent(worldPos));
        }
    }
}