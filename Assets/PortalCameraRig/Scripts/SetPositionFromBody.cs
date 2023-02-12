using System.Collections;
using System.Collections.Generic;
using com.rfilkov.kinect;
using UnityEngine;

public class SetPositionFromBody : MonoBehaviour
{
    // Start is called before the first frame update
    public PortalCameraController portalManager;

    // Update is called once per frame
    void Update()
    {
        KinectManager kinectManager = KinectManager.Instance;
        Transform kinectTransform = kinectManager.transform;
        // get 1st player
        var index = 0;
        ulong userID = kinectManager ? kinectManager.GetUserIdByIndex(index) : 0;

        if (kinectManager.IsJointTracked(userID, 25) && kinectManager.IsJointTracked(userID, 27))
        {
            
            Vector3 leftEye = kinectManager.GetJointPosition(userID, 25);
            Vector3 rightEye = kinectManager.GetJointPosition(userID, 26);
            
            Vector3 leftEyeMirrored = new Vector3(-leftEye.x, leftEye.y, leftEye.z);
            Vector3 rightEyeMirrored = new Vector3(-rightEye.x, rightEye.y, rightEye.z);   
            
            
            //get midpoint betweeen eyes
            Vector3 midpoint = (leftEyeMirrored + rightEyeMirrored) / 2;
            
            transform.localPosition = midpoint * portalManager.scalingFactor;
        }
    }

}
