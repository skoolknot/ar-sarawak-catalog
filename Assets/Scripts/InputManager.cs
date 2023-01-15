using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _title;

    [SerializeField]
    private Button _button;

    [SerializeField]
    private GameObject _cube;

    [SerializeField]
    private float _multiplier = 100.0f;

    private int _tapCount = 0;

    private void OnEnable()
    {
        UpdateTapCount();

        _button.onClick.AddListener(() => AddForce());
    }

    private void AddForce()
    {
        Debug.Log("Add force.");

        _tapCount += 1;

        UpdateTapCount();
        AddForceToCube();
    }

    private void UpdateTapCount() => _title.text = $"Tap: {_tapCount}";

    private void AddForceToCube()
    {
        if (_cube.gameObject.TryGetComponent(out Rigidbody rigidbody))
            rigidbody.AddForce(Vector3.up * _multiplier);
    }
}
