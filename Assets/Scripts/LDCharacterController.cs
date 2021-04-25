using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LDCharacterController : MonoBehaviour
{
    public CinemachineVirtualCamera activeCamera;
    public LayerMask groundMask;
    public bool isGrounded = false;
    public float isGroundedTimer = 0.0f;
    public float jumpLeeway = 0.2f;
    public float checkRadius = .021f;
    public Transform spawnPoint;
    public GameObject parentWithRespawnables;

    Vector3 move;

    public bool shouldJump = false;
    public bool mineJump = false;
    bool mineJumpActive = false;

    private CharacterController characterController;
    public bool respawning;

    private Animator anim;

    //public GameObject groundCheck;

    public LDCharacterStats stats;

    private LDCharacterInput input;

    public List<Transform> groundChecks;
    public List<Transform> wallChecks;
    public List<Transform> ceilingChecks;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        stats = GetComponent<LDCharacterStats>();
        input = GetComponent<LDCharacterInput>();

    }

    void Start()
    {
        move = Vector3.zero;
        input.OnLeftPressed += HandleLeftPressed;
        input.OnRightPressed += HandleRightPressed;
        input.OnHorizontalReleased += HandleHorizontalReleased;
        input.OnRetryPressed += HandleRetryPressed;
    }



    // Update is called once per frame
    void Update()
    {
        if(!respawning)
        {


            bool checkGround = Physics.CheckSphere(transform.position, checkRadius, groundMask, QueryTriggerInteraction.Ignore);

            foreach (var check in groundChecks)
            {
                if (Physics.CheckSphere(check.transform.position, checkRadius, groundMask, QueryTriggerInteraction.Ignore))
                {
                    checkGround = true;
                    break;
                }
            }
            if (!checkGround && isGrounded) //walked off
            {
                StartCoroutine(ResetIsGrounded(jumpLeeway));
            }
            else if(checkGround) //on ground
            {
                isGrounded = true;
            }
            else // in air
            {
                move.y += stats.gravityFactor * Time.deltaTime;
                if (move.y < stats.terminalVerticalVelocity)
                    move.y = stats.terminalVerticalVelocity;

                if(move.y > 0) //ceilingChecks
                {
                    foreach (var check in ceilingChecks)
                    {
                        if (Physics.CheckSphere(check.transform.position, checkRadius, groundMask, QueryTriggerInteraction.Ignore))
                        {
                            move.y = 0;
                            break;
                        }
                    }
                }
            }

            bool blocked = false;

            foreach (var check in wallChecks)
            {
                if (Physics.CheckSphere(check.transform.position, .01f, groundMask, QueryTriggerInteraction.Ignore))
                {
                    blocked = true;
                    break;
                }
            }

            if (!blocked)
            {
                characterController.Move(move.With(y: 0) * Time.deltaTime);
            }

            if(shouldJump || mineJump)
            {
                move.y += Mathf.Sqrt(stats.jumpPower * -2 * stats.gravityFactor * ((mineJump) ? 2 : 1));
                shouldJump = false;
                if (mineJump)
                    mineJumpActive = true;
                else
                {
                    AudioManager.instance.Play("Jump");
                }
                mineJump = false;
                isGrounded = false;
            }
            else if(isGrounded)
            {
                move.y = 0;
            }
            characterController.Move(move.With(x: 0) * Time.deltaTime);

            if (mineJumpActive && move.y < 0)
                mineJumpActive = false;


            anim.SetFloat("Speed", Mathf.Abs(move.x));
            anim.SetBool("IsGrounded", isGrounded);
            anim.SetFloat("VerticalSpeed", move.y);
        }
    }

    IEnumerator ResetIsGrounded(float time)
    {
        yield return new WaitForSeconds(time);
        var checkGround = Physics.CheckSphere(transform.position, checkRadius, groundMask, QueryTriggerInteraction.Ignore); Physics.CheckSphere(transform.position, .021f, groundMask, QueryTriggerInteraction.Ignore);

        foreach (var check in groundChecks)
        {
            if (Physics.CheckSphere(check.transform.position, checkRadius, groundMask, QueryTriggerInteraction.Ignore))
            {
                checkGround = true;
                break;
            }
        }
        isGrounded = checkGround;
        anim.SetBool("IsGrounded", isGrounded);
    }

    public void JumpCancelled()
    {
        if (move.y > 0 && !mineJumpActive)
            move.y = 0;
    }

    void HandleRightPressed()
    {
        move.x = stats.runSpeed;
        transform.rotation = Quaternion.Euler(0, 90, 0);
    }
    void HandleLeftPressed()
    {

        move.x = -stats.runSpeed;
        transform.rotation = Quaternion.Euler(0, -90, 0);
    }
    void HandleHorizontalReleased()
    {

        move.x = 0;
        //characterController.Move(move.With(x: 0));
        anim.SetFloat("Speed", Mathf.Abs(move.x));
        //transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    void HandleRetryPressed()
    {
        Respawn();
    }

    public void Respawn()
    {
        characterController.enabled = false;
        transform.position = spawnPoint.position;
        characterController.enabled = true;

        foreach(var r in parentWithRespawnables.GetComponentsInChildren<ReactivateChildren>())
        {
            r.Reactivate();
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Death"))
        {
            StartCoroutine(TriggerRespawn(.3f));
        }
    }


    private IEnumerator TriggerRespawn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Respawn();
    }
}
