using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public static CameraController Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<CameraController>();
                if(instance == null)
                {
                    GameObject go = new GameObject("CameraController");
                    instance = go.AddComponent<CameraController>();
                    DontDestroyOnLoad(go);
                }
            }    
            return instance;
        }
    }
    public Transform target;
    public float SmoothSpeed = 0.125f;
    public Vector3 Offset;
    public bool StopCam = false;

    private void FixedUpdate()
    {
        if(!StopCam)
        {
            Vector3 desiredPosi = target.position + Offset;
            Vector3 smoothPosi = Vector3.Lerp(transform.position, desiredPosi, SmoothSpeed);
            transform.position = smoothPosi;
        }
    }
}
