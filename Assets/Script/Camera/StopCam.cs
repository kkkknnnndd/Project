using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCam : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Wall")
        {
            CameraController.Instance.StopCam = true;
            Debug.Log(CameraController.Instance.StopCam);
        }
        else if(other.tag == "Stop_Cam_Track_90")
        {
            Stop_Cam_Track(90f);
        } 
        else if(other.tag == "Stop_Cam_Track_0")
        {
            Stop_Cam_Track(0f);
        }
        else if(other.tag == "Stop_0_90")
        {
            Stop_Cam_Track(90f);
            Stop_Cam_Track(0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wall")
        {
            CameraController.Instance.StopCam = false;
        }
        else if(other.tag == "Stop_Cam_Track_90" || other.tag == "Stop_Cam_Track_0" 
            || other.tag == "Stop_0_90")
        {
            Cam_Track.Instance.stop_Cam = false;
        }
    }

    private void Stop_Cam_Track(float num)
    {
        if(transform.localEulerAngles.y == num)
        {
            Cam_Track.Instance.stop_Cam = true;
        }
    }
}
