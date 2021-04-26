using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public LDCharacterController controller;


    private void Start()
    {
        if (controller == null)
            controller = GetCommonEntities.instance.controller;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            controller.mineJump = true;
            AudioManager.instance.Play("Mine");
            gameObject.SetActive(false);
        }
    }
}
