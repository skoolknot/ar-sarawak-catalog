using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
    [SerializeField]
    private Vector3 _rotation = Vector3.zero;

    [SerializeField]
    private float _rotationSpeed = 1.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(_rotation * _rotationSpeed * Time.deltaTime);
    }
}
