using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCameraController : MonoBehaviour
{
    [Tooltip("change the scaling of the content")]
    [Range(0.001f, 100.0f)]
    public float scalingFactor = 1.0f;

    [Tooltip("physical width of the screen in meters")]
    public float screenWidth = 1.42f;
    
    [Tooltip("physical height of the screen in meters")]
    public float screenHeight = 0.84f;
    
    [Tooltip("position of the kinect in relation to the screen center")]
    public Vector3 kinectPosition = new Vector3(0.0f,0.44f,0.0f);
    
    [Tooltip("rotation of the kinect relative to screen")]
    public Vector3 kinectRotation = new Vector3(0.0f,180.0f, 0.0f);
    
    [Tooltip("show the crosshair reference to measure the kinect position from")]
    public bool showCrosshair = true;

    public GameObject screen;
    public GameObject crosshair;
    public GameObject kinect;
    
    
    //Set Screen Size when values changes in the inspector
    private void OnValidate()
    {
        //apply values when they are changed in the inspector. When changing these values from script, make sure to call the methods afterwards!
        UpdateValues();
    }

    private void UpdateValues()
    {
        kinect.transform.SetPositionAndRotation(kinectPosition,Quaternion.Euler(kinectRotation));
        crosshair.SetActive(showCrosshair);
        screen.transform.localScale = new Vector3(screenWidth * scalingFactor, screenHeight * scalingFactor, 0.01f);
    } 
}
