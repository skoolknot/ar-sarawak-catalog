using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleForce : MonoBehaviour
{
    [SerializeField]
    private InputAction _tapAction;

    [SerializeField]
    private float _multiplier = 100.0f;

    private void OnEnable()
    {
        _tapAction.Enable();
    }

    private void Start()
    {
        _tapAction.started += OnTapAction;
    }

    private void OnDisable()
    {
        _tapAction.started -= OnTapAction;

        _tapAction.Disable();
    }

    private void OnTapAction(InputAction.CallbackContext ctx)
    {
        if (TryGetComponent(out Rigidbody rigidbody))
            rigidbody.AddForce(Vector3.up * _multiplier);
    }
}
