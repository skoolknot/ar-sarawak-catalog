using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlane : MonoBehaviour
{
    [SerializeField]
    private InputAction _tapAction;

    [SerializeField]
    private GameObject _objectPrefab;

    [SerializeField]
    private GameObject _placementIndicator;

    private ARRaycastManager _raycastManager;

    private bool _placementPoseValid = false;
    private Pose _placementPose;
    private bool _isObjectPlaced = false;

    private void Awake() => _raycastManager = GetComponent<ARRaycastManager>();

    private void OnEnable() => _tapAction.Enable();

    private void Start() => _tapAction.started += OnTap;

    private void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }

    private void OnDisable()
    {
        _tapAction.started -= OnTap;

        _tapAction.Disable();
    }

    private void OnTap(InputAction.CallbackContext ctx)
    {
        if (_placementPoseValid && !_isObjectPlaced)
            PlaceObject();
    }

    private void UpdatePlacementPose()
    {
        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        List<ARRaycastHit> hits = new();
        if (_raycastManager.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            _placementPoseValid = hits.Count > 0;
            if (_placementPoseValid)
            {
                _placementPose = hits[0].pose;

                Vector3 cameraForward = Camera.current.transform.forward;
                Vector3 cameraBearing = new Vector3(cameraForward.x, 0.0f, cameraForward.z).normalized;
                _placementPose.rotation = Quaternion.LookRotation(cameraBearing);
            }
        }
    }

    private void UpdatePlacementIndicator()
    {
        if (_placementPoseValid && !_isObjectPlaced)
        {
            _placementIndicator.SetActive(true);
            _placementIndicator.transform.SetPositionAndRotation(_placementPose.position, _placementPose.rotation);
        }
        else
        {
            _placementIndicator.SetActive(false);
        }
    }

    private void PlaceObject()
    {
        _placementPoseValid = false;
        _isObjectPlaced = true;

        Instantiate(_objectPrefab, _placementPose.position, _placementPose.rotation);
    }
}
