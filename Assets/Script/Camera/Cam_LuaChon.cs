using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Cam_Track : MonoBehaviour
{
    public static Cam_Track instance;
    public static Cam_Track Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Cam_Track>();
                if (instance == null)
                {
                    GameObject go = new GameObject("Cam_Track");
                    instance = go.AddComponent<Cam_Track>();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }
    public Transform trackedObject;
    public float maxDistance;
    public float updateSpeed;
    public float hight;
    public bool stop_Cam = false;
    [Range(0, 10)]
    public float currentDistance = 5;

    void LateUpdate()
    {
        //ahead.transform.position = trackedObject.position + trackedObject.forward * (maxDistance * 0.25f);
        if(!stop_Cam)
        {
            Vector3 desiredPosi = trackedObject.position + Vector3.up * currentDistance 
                - trackedObject.forward * (currentDistance + maxDistance * 0.5f);
            desiredPosi.y *= hight;
            Vector3 smoothPosi = Vector3.Lerp(transform.position, desiredPosi, updateSpeed * Time.deltaTime);
            transform.position = smoothPosi;
            transform.LookAt(trackedObject);
        }
    }
}
