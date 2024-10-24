using System;
using DG.Tweening;
using Game;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    public class InputManager : MonoBehaviour
    {
        private InputScheme _inputScheme;

        private void Awake()
        {
            _inputScheme = new InputScheme();
            _inputScheme.Enable();
            
            _inputScheme.Map.ClickLeftButton.performed += OnClick;
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