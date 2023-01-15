using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotateManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _cube;

    [SerializeField]
    private Vector3 _rotation = Vector3.zero;

    [SerializeField]
    private float _rotationSpeed = 1.0f;

    void Update()
    {
        _cube.transform.Rotate(_rotation * _rotationSpeed * Time.deltaTime);
    }
}
