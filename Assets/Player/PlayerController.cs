using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform; //player Transform
    [SerializeField]
    protected Rigidbody2D playerBody; // player Rigidbody
    [SerializeField]
    private CapsuleCollider2D feet; // player capsule collider for bottom part
    [SerializeField]
    private Transform playerSpriteTransform; // position of sprites (for animation handling)
    [SerializeField]
    float playerSpeed; // set the speed that player will move
    [SerializeField]
    float changibleGravityStrenght; // how strong will the gravity be
    [SerializeField]
    float jumpForce; // how strong is player jump
    [SerializeField]
    float jumpCount;
    [SerializeField]
    float dashStrenght;
    [SerializeField]
    LayerMask WhatIsGround; // layer that separets ground from everything else
    [SerializeField]
    private float jumpTime; // how long can player jump for
    [SerializeField]
    private float dashTime;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Transform ceilingCheck;
    [SerializeField]
    private int fpsSet;
    [SerializeField]
    private Animator animator;


    public float jumpTimeCounter; // how long is player already jumping for

    public float dashTimeCounter;

    public bool pressedJump; // update check for spacebar press
    public bool holdingJump; // update check for spacebar hold
    public bool releasedJump;
    public bool pressedDash;

    public bool isDashing = false;
    public bool isJumping;
    public bool isDoingDouble;
    public bool grounded; // is player on ground?

    public float fps;
    public float time;

    private float fallSpeedYDdampingTreshold;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = fpsSet;
        fallSpeedYDdampingTreshold = CameraManager.instance.fallSpeedDampingChangeTreshold;
    }



    // Update is called once per frame
    void Update()
    {
        GroundedManagment();

        animator.SetFloat("PlayerSpeed", Mathf.Abs(playerBody.linearVelocityX));
        animator.SetFloat("PlayerFallSpeed", playerBody.linearVelocityY);

        Debug.DrawLine(playerTransform.position, playerSpriteTransform.position, Color.green, 2); // just to visualize movement of player

        

        SpaceBarPressed();
        JumpKeyReleased();
        holdingJump = SpaceBarHold();
        DashKeyPressed();

        CeilingHit();

        time += Time.deltaTime; //session time

        fps = Application.targetFrameRate; // fps value (pre set)

        
        ManageCameras();
        
    }

    private void ManageCameras()
    {
        // if player is falling with at least ___ speed
        if (playerBody.linearVelocity.y < fallSpeedYDdampingTreshold && !CameraManager.instance.IsLerpingYDamping && !CameraManager.instance.LerpedFromLayerFalling)
        {
            CameraManager.instance.LerpYDamping(true);
        }

        // if standing still or moving up
        if (playerBody.linearVelocity.y >= 0f && !CameraManager.instance.IsLerpingYDamping && CameraManager.instance.LerpedFromLayerFalling)
        {
            //reset so it can be called again
            CameraManager.instance.LerpedFromLayerFalling = false;

            CameraManager.instance.LerpYDamping(false);
        }
    }

   

    private void CeilingHit()
    {
        bool hitCeling = Physics2D.OverlapBox(ceilingCheck.position, feet.size * 0.85f, 0f, WhatIsGround);

        var velocity = playerBody.linearVelocity;

        if (hitCeling)
        {
            velocity.y = -0.01f;
            velocity = playerBody.linearVelocity;
            isJumping = false;
        }
    }


    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, feet.size * 0.85f, 0f, WhatIsGround);
    }

    private void SpaceBarPressed()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (grounded || jumpCount > 0)){
            pressedJump = true;

        }
    }

    private void JumpKeyReleased()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            releasedJump = true;
        }
    }

    private void DashKeyPressed()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            pressedDash = true;
 
        }

        
    }

    private bool SpaceBarHold()
    {
        return Input.GetKey(KeyCode.Space);
    }

    private void FixedUpdate()
    {
        GroundedManagment();

        

        if (!isDashing)
        {
            Move(); // movement to left and right
            Jump();
        }
        Dash();
    }

    private void GroundedManagment()
    {
        grounded = IsGrounded();

        if (grounded)
        {

            jumpTimeCounter = jumpTime;
            if (grounded || isDashing)
            {
                playerBody.gravityScale = 0;
            }
            animator.SetBool("Grounded", true);
            jumpCount = 1;
            if (!isDashing)
            {
                dashTimeCounter = dashTime;
            }
        }
        else
        {
            if (!isDashing)
            {
                playerBody.gravityScale = 10;
            }
            animator.SetBool("Grounded", false);
        }
    }

    private void Move()
    {
        //get imput for moving right and left
        float calculatedMove = Input.GetAxisRaw("Horizontal") * playerSpeed;

        //move player
        playerBody.linearVelocity = new Vector2(calculatedMove, playerBody.linearVelocity.y);

        

        //rotate player based on input
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            playerTransform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            playerTransform.rotation = Quaternion.Euler(0, 0, 0);
        }

    }

    private void Dash()
    {
        float rotationMUltiplier;

        if(playerTransform.rotation.y < 0)
        {
            rotationMUltiplier = -1;
        }
        else
        {
            rotationMUltiplier = 1;
        }

        if (pressedDash)
        {
            isDashing = true;
            isJumping = false;
        }

        if(isDashing && dashTimeCounter > 0)
        {
            playerBody.linearVelocity = new Vector2 (dashStrenght * rotationMUltiplier, 0);
            dashTimeCounter -= Time.deltaTime;
            playerBody.gravityScale = 0;

                
        }else
        {
            isDashing = false;
            pressedDash = false;
        }

            
        
    }


    private void Jump()
    {
        //working jump? WORKING JUMP YAYYYYYYYYYYYYYYYYYYYY

        
        var velocity = playerBody.linearVelocity;
        
 
        if(pressedJump && grounded)
        {
            isJumping = true;
            pressedJump = false;
            
        }

        if (pressedJump && !grounded && jumpCount > 0)
        {
            isDoingDouble = true;
        }

        
        
        if (holdingJump && jumpTimeCounter > 0)
        {
            if (isJumping)
            {
                velocity.y = jumpForce;
                playerBody.linearVelocity = velocity;
                jumpTimeCounter -= Time.deltaTime;

            }
            else
            {
                isJumping = false;
            }
        }

        if (isDoingDouble)
        {
            velocity.y = jumpForce;
            playerBody.linearVelocity = velocity;
            pressedJump = false;
            isDoingDouble = false;
            jumpCount -= 1;
        }

        if (releasedJump)
        {
            isJumping = false;
            releasedJump = false;
        }

        

        //Debug.Log("Je ve skoku" + isJumping + " " + jumpTimeCounter);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheck.position, feet.size * 0.85f);
        Gizmos.DrawWireCube(ceilingCheck.position, feet.size * 0.85f);
    }

}