using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTranslate : MonoBehaviour
{
    [SerializeField]
    private Vector3 _translate;

    [SerializeField]
    private float _translateSpeed;

    void Update()
    {
        transform.Translate(_translate * _translateSpeed * Time.deltaTime);
    }
}
