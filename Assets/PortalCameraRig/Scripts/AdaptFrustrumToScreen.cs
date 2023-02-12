using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptFrustrumToScreen : MonoBehaviour
{
    public Transform ScreenTransform;
    private Camera _camera;

    private void SetFov()
    {
        //Takes the size and position of the screen and calculates the field of view based on the distance to the camera
        float distance = Vector3.Distance(ScreenTransform.position, _camera.transform.position);
        float height = ScreenTransform.localScale.y;
        float fov = 2 * Mathf.Atan(height / (2 * distance)) * Mathf.Rad2Deg;
        _camera.fieldOfView = fov;
    }
    
    private void SetCameraDirection()
    {
        //Sets the camera to look at the screen
        _camera.transform.LookAt(ScreenTransform);
    }

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        SetFov();
        SetCameraDirection();
    }
}
