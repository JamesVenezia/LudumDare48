using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class ConfinerTransition : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    public LDCharacterController controller;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            controller.activeCamera.gameObject.SetActive(false);
            controller.activeCamera = cam;
            cam.gameObject.SetActive(true);
        }
    }
}
