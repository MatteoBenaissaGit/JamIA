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
            
            _inputScheme.Map.ClickLeftButton.performed += OnClickLeftButton;
            _inputScheme.Map.ClickLeftButton.canceled += OnReleaseLeftButton;
        }

        private void OnReleaseLeftButton(InputAction.CallbackContext obj)
        {
            if (GameManager.Instance.IsHoldingSeed == false)
            {
                return;
            }

            HasBlockOnMouse(out BlockController block);
            GameManager.Instance.DropSeedOnBlock(block);
        }

        private void OnClickLeftButton(InputAction.CallbackContext obj)
        {
            
        }

        public bool HasBlockOnMouse(out BlockController block)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject.TryGetComponent(out block))
            {
                return true;
            }

            block = null;
            return false;
        }
    }
}