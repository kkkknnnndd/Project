using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DoiHuong : MonoBehaviour
{
    private bool isPress_R = false;
    public static bool _dhChangeScene = false;
    private CharacterController characterController;
    public Camera cam0;
    public Camera cam90;
    public Camera camChoose;
    private void Start()
    {
        GameObject temp = GameObject.FindGameObjectWithTag("Player");
        characterController = temp.GetComponent<CharacterController>();
        Doi_Cam();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Press R");
            isPress_R = true;
        }
        DoiCamForSceneTrig();
    }
    private void OnTriggerEnter(Collider other)
    {
       if(other.tag == "ChangeHuong")
       {
            Camera.main.GetComponent<AudioListener>().enabled = false;
            Camera.main.gameObject.SetActive(false);
            camChoose.gameObject.SetActive(true);
            camChoose.GetComponent<AudioListener>().enabled = true;
       }    
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "ChangeHuong" && isPress_R)
        {
            Debug.Log("In");
            switch (transform.localEulerAngles.y)
            {
                case 0:
                    Xoay_Chuyen(90f);
                    break;
                case 90f:
                    Xoay_Chuyen(0);
                    break;
            }
            characterController.enabled = false;
            transform.position = other.transform.position;
            characterController.enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "ChangeHuong")
        {
            Doi_Cam();
        }    
    }
    private void Xoay_Chuyen(float num)
    {
        var current_rota = transform.localEulerAngles;
        current_rota.y = num;
        transform.localEulerAngles = current_rota;
        isPress_R = false;
    }    
    public void Doi_Cam()
    {
        camChoose.gameObject.SetActive(false);
        if (characterController.transform.localEulerAngles.y == 0)
        {
            cam0.gameObject.SetActive(true);
            cam0.GetComponent<AudioListener>().enabled = true;
            cam90.gameObject.SetActive(false);
        }
        else
        {
            cam90.gameObject.SetActive(true);
            cam90.GetComponent<AudioListener>().enabled = true;
            cam0.gameObject.SetActive(false);
        }
    }  
    private void DoiCamForSceneTrig()
    {
        if(_dhChangeScene)
        {
            Doi_Cam();
            _dhChangeScene = false;
        }
    }
}
