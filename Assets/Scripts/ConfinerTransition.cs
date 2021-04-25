using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class ConfinerTransition : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    public LDCharacterController controller;
    public Transform spawnPoint;
    public GameObject parentWithRespawnables;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            controller.activeCamera.gameObject.SetActive(false);
            controller.activeCamera = cam;
            cam.gameObject.SetActive(true);
            controller.spawnPoint = spawnPoint;
            controller.parentWithRespawnables = parentWithRespawnables;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}
