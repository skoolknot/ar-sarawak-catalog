using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _cube;

    [SerializeField]
    private InputAction _primaryContact;

    [SerializeField]
    private InputAction _primaryPosition;

    [SerializeField]
    private InputAction _secondaryContact;

    [SerializeField]
    private InputAction _secondaryPosition;

    [SerializeField]
    private float _rotationSpeed = 1.0f;

    private bool _isPrimaryContact = false;
    private bool _isSecondaryContact = false;

    private Vector2 _currentPrimaryViewportPoint = Vector2.zero;
    private Vector2 _currentSecondaryViewportPoint = Vector2.zero;

    private float _previousPrimaryViewportPointX = 0.0f;
    private Quaternion _cubeRotationY = Quaternion.identity;

    private float _initialDistanceBetweenViewportPoints = 0.0f;
    private float _currentDistanceBetweenViewportPoints = 0.0f;
    private Vector3 _cubeInitialScale = Vector3.zero;

    private void OnEnable()
    {
        _primaryContact.Enable();
        _primaryPosition.Enable();

        _secondaryContact.Enable();
        _secondaryPosition.Enable();
    }

    private void Start()
    {
        _primaryContact.started += OnPrimaryContactStarted;
        _primaryContact.canceled += OnPrimaryContactEnded;

        _secondaryContact.started += OnSecondaryContactStarted;
        _secondaryPosition.canceled += OnSecondaryContactEnded;
    }

    private void OnDisable()
    {
        _primaryContact.started -= OnPrimaryContactStarted;
        _primaryContact.canceled -= OnPrimaryContactEnded;

        _secondaryContact.started -= OnSecondaryContactStarted;
        _secondaryPosition.canceled -= OnSecondaryContactEnded;

        _primaryContact.Disable();
        _primaryPosition.Disable();

        _secondaryContact.Disable();
        _secondaryPosition.Disable();
    }

    private void OnPrimaryContactStarted(InputAction.CallbackContext ctx)
    {
        _isPrimaryContact = true;

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

            if (!_isSecondaryContact)
                RotateObject();
        }
    }

    private void OnSecondaryContactStarted(InputAction.CallbackContext ctx)
    {
        _isSecondaryContact = true;

        _secondaryPosition.performed += OnSecondaryPositionPerformed;
    }

    private void OnSecondaryContactEnded(InputAction.CallbackContext ctx)
    {
        _isSecondaryContact = false;

        _initialDistanceBetweenViewportPoints = 0.0f;
        _cubeInitialScale = Vector3.zero;

        _secondaryPosition.performed -= OnSecondaryPositionPerformed;
    }

    private void OnSecondaryPositionPerformed(InputAction.CallbackContext ctx)
    {
        if (_isSecondaryContact)
        {
            _currentSecondaryViewportPoint = Camera.main.ScreenToViewportPoint(ctx.ReadValue<Vector2>());

            if (_initialDistanceBetweenViewportPoints == 0.0f && _cubeInitialScale == Vector3.zero)
            {
                _initialDistanceBetweenViewportPoints = GetDistanceBetweenViewportPoints();
                _cubeInitialScale = _cube.transform.localScale;
            }

            ScaleObject();
        }
    }

    private void RotateObject()
    {
        int direction = _currentPrimaryViewportPoint.x > _previousPrimaryViewportPointX ? -1 : 1;
        _cubeRotationY = Quaternion.Euler(0.0f, _currentPrimaryViewportPoint.x * direction * _rotationSpeed, 0.0f);
        _previousPrimaryViewportPointX = _currentPrimaryViewportPoint.x;

        _cube.transform.rotation = _cubeRotationY * _cube.transform.rotation;
    }

    private void ScaleObject()
    {
        _currentDistanceBetweenViewportPoints = GetDistanceBetweenViewportPoints();

        if (_initialDistanceBetweenViewportPoints != 0.0f)
        {
            float factor = _currentDistanceBetweenViewportPoints / _initialDistanceBetweenViewportPoints;
            _cube.transform.localScale = _cubeInitialScale * factor;
        }
    }

    private float GetDistanceBetweenViewportPoints()
    {
        if (_currentPrimaryViewportPoint != Vector2.zero && _currentSecondaryViewportPoint != Vector2.zero)
            return Vector2.Distance(_currentPrimaryViewportPoint, _currentSecondaryViewportPoint);
        else
            return 0.0f;
    }
}
