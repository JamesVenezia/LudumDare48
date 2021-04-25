using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public LDCharacterController controller;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            controller.mineJump = true;
            gameObject.SetActive(false);
        }
    }
}
