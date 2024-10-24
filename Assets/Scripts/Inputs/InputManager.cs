using System;
using DG.Tweening;
using Game;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        public InputScheme InputScheme => _inputScheme;

        private InputScheme _inputScheme;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
            
            _inputScheme = new InputScheme();
            _inputScheme.Enable();
            
            _inputScheme.Map.ClickLeftButton.started += OnClick;
        }

        private void OnClick(InputAction.CallbackContext context)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject.TryGetComponent(out BlockController block))
            {
                GameObject blockObject = block.gameObject;
            }
        }
    }
}