using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LDCharacterInput : MonoBehaviour
{


    public event Action OnRightPressed = delegate { };
    public event Action OnHorizontalReleased = delegate { };
    public event Action OnLeftPressed = delegate { };
    public event Action OnRetryPressed = delegate { };


    LDCharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<LDCharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            controller.shouldJump = true;
        }
        else if(!Input.GetButton("Jump") && !controller.isGrounded)
        {
            controller.JumpCancelled();
        }

        if (Input.GetAxisRaw("Horizontal") < 0f)
        {
            // left
            OnLeftPressed();
        }
        else if(Input.GetAxisRaw("Horizontal") > 0.0f)
        {
            OnRightPressed();
        }
        else
        {
            OnHorizontalReleased();
        }

        if(Input.GetButtonDown("Retry"))
        {
            OnRetryPressed();
        }

    }
}
