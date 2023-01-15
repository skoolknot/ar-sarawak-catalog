using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ARInteractionManager : MonoBehaviour
{
    [SerializeField]
    private InputAction _primaryContact;

    [SerializeField]
    private InputAction _primaryPosition;

    [SerializeField]
    private float _rotationSpeed = 1.0f;

    private bool _isPrimaryContact = false;

    private Vector2 _currentPrimaryViewportPoint = Vector2.zero;

    private float _previousPrimaryViewportPointX = 0.0f;
    private Quaternion _cubeRotationY = Quaternion.identity;

    private GameObject _arObject;

    private void OnEnable()
    {
        _primaryContact.Enable();
        _primaryPosition.Enable();
    }

    private void Start()
    {
        _primaryContact.started += OnPrimaryContactStarted;
        _primaryContact.canceled += OnPrimaryContactEnded;
    }

    private void OnDisable()
    {
        _primaryContact.started -= OnPrimaryContactStarted;
        _primaryContact.canceled -= OnPrimaryContactEnded;

        _primaryContact.Disable();
        _primaryPosition.Disable();
    }

    private void OnPrimaryContactStarted(InputAction.CallbackContext ctx)
    {
        _isPrimaryContact = true;

        _arObject = GameObject.FindGameObjectWithTag("ARObject");

        _primaryPosition.performed += OnPrimaryPositionPerformed;
    }

    private void OnPrimaryContactEnded(InputAction.CallbackContext ctx)
    {
        _isPrimaryContact = false;

        _primaryPosition.performed -= OnPrimaryPositionPerformed;
    }

    private void OnPrimaryPositionPerformed(InputAction.CallbackContext ctx)
    {
        if (_isPrimaryContact)
        {
            _currentPrimaryViewportPoint = Camera.main.ScreenToViewportPoint(ctx.ReadValue<Vector2>());

            RotateObject();
        }
    }

    private void RotateObject()
    {
        if (_arObject != null)
        {
            int direction = _currentPrimaryViewportPoint.x > _previousPrimaryViewportPointX ? -1 : 1;
            _cubeRotationY = Quaternion.Euler(0.0f, _currentPrimaryViewportPoint.x * direction * _rotationSpeed, 0.0f);
            _previousPrimaryViewportPointX = _currentPrimaryViewportPoint.x;

            _arObject.transform.rotation = _cubeRotationY * _arObject.transform.rotation;
        }
    }
}
