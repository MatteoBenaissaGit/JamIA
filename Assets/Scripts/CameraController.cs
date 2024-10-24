using System;
using DG.Tweening;
using Inputs;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This class manage the camera used in the level editor
/// </summary>
public class CameraController : MonoBehaviour
{
    [field: SerializeField] public Camera Camera { get; private set; }

    [SerializeField] private Transform _cameraParent;

    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _zoomSpeed = 0.1f;
    [SerializeField] private float _cameraSizeMin = 0.5f;
    [SerializeField] private float _cameraSizeMax = 10f;
    [SerializeField] private float _moveSpeedMultiplierPerZoom = 2f;
    [SerializeField] private float _edgeMovementSpeed = 2f;

    private bool _doMoveCamera;
    private Vector2 _cameraMovement;
    private float _cameraZoom;
    private float _baseSize;
    private Camera _cardCamera;
    private Vector2Int _edgeMovement;

    [SerializeField] private float _firstBorderThickness;
    [SerializeField] private float _secondaryBorderThickness;

    private void Start()
    {
        _baseSize = Camera.orthographicSize;

        if (InputManager.Instance == null)
        {
            throw new Exception("no input manager");
        }

        InputManager.Instance.InputScheme.Map.CameraMoveButton.started += (_) => _doMoveCamera = true;
        InputManager.Instance.InputScheme.Map.CameraMoveButton.canceled += (_) => _doMoveCamera = false;

        InputManager.Instance.InputScheme.Map.CameraMovement.performed +=
            (ctx) => _cameraMovement = ctx.ReadValue<Vector2>();
        InputManager.Instance.InputScheme.Map.CameraMovement.canceled += (_) => _cameraMovement = Vector2.zero;

        InputManager.Instance.InputScheme.Map.Scroll.performed += (ctx) => _cameraZoom = ctx.ReadValue<float>();
        InputManager.Instance.InputScheme.Map.Scroll.canceled += (ctx) => _cameraZoom = 0;

        InputManager.Instance.InputScheme.Map.CameraMovement.performed += ctx => EdgeMovement();
    }

    private void Update()
    {
        if (EventSystem.current == null || EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Camera.DOKill();
        float size = Camera.orthographicSize - _cameraZoom * _zoomSpeed;
        size = Mathf.Clamp(size, _cameraSizeMin, _cameraSizeMax);
        Camera.DOOrthoSize(size, 0.1f).SetEase(Ease.Flash);

        if (_doMoveCamera == false)
        {
            EdgeScrolling();
            return;
        }

        MoveCameraWithMovement(_cameraMovement);
    }

    private bool IsMouseOverGameWindow()
    {
        Vector3 mp = Input.mousePosition;
        return !(mp.x < 0) && !(mp.x > Screen.width) && !(mp.y < 0) && !(mp.y > Screen.height);
    }

    private void EdgeScrolling()
    {
        if (IsMouseOverGameWindow() == false || _edgeMovement == Vector2Int.zero)
            return;

        Vector2 movement = new Vector2(_edgeMovement.x * _edgeMovementSpeed, _edgeMovement.y * _edgeMovementSpeed);
        MoveCameraWithMovement(movement);

        Debug.Log(movement);
    }

    private void MoveCameraWithMovement(Vector2 movement)
    {
        float zoomFactor = Camera.orthographicSize / _baseSize;
        float zoomFactorSpeed = zoomFactor * _moveSpeedMultiplierPerZoom;
        float deltaTime = Time.deltaTime * Screen.dpi;
        Vector3 xMovement = Camera.transform.right * (-movement.x * _moveSpeed * zoomFactorSpeed * deltaTime);
        Vector3 yMovement = Camera.transform.up * (-movement.y * _moveSpeed * zoomFactorSpeed * deltaTime);
        Camera.transform.localPosition += xMovement + yMovement;
    }

    private void EdgeMovement()
    {
        Vector2Int edgeVector = Vector2Int.zero;

        if (Input.mousePosition.y >= Screen.height - _firstBorderThickness)
        {
            edgeVector.y = -1;
        }
        else if (Input.mousePosition.y <= _firstBorderThickness)
        {
            edgeVector.y = 1;
        }

        if (Input.mousePosition.x >= Screen.width - _firstBorderThickness)
        {
            edgeVector.x = -1;
        }
        else if (Input.mousePosition.x <= _firstBorderThickness)
        {
            edgeVector.x = 1;
        }

        if (edgeVector.x != 0)
        {
            if (Input.mousePosition.y >= Screen.height - _secondaryBorderThickness)
            {
                edgeVector.y = -1;
            }
            else if (Input.mousePosition.y <= _secondaryBorderThickness)
            {
                edgeVector.y = 1;
            }
        }
        else if (edgeVector.y != 0)
        {
            if (Input.mousePosition.x >= Screen.width - _secondaryBorderThickness)
            {
                edgeVector.x = -1;
            }
            else if (Input.mousePosition.x <= _secondaryBorderThickness)
            {
                edgeVector.x = 1;
            }
        }

        _edgeMovement = edgeVector;
    }
}