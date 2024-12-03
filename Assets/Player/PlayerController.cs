using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform; //player Transform
    [SerializeField]
    protected Rigidbody2D playerBody; // player Rigidbody
    [SerializeField]
    private CapsuleCollider2D feet;
    [SerializeField]
    private Transform playerSpriteTransform; // position of sprites (for animation handling)
    [SerializeField]
    float playerSpeed; // set the speed that player will move
    [SerializeField]
    float changibleGravityStrenght; // how strong will the gravity be
    [SerializeField]
    float jumpForce; // how strong is player jump
    [SerializeField]
    LayerMask WhatIsGround; // layer that separets ground from everything else
    [SerializeField]
    private float jumpTime; // how long can player jump for
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Transform ceilingCheck;
    [SerializeField]
    private int fpsSet;
    public float jumpTimeCounter; // how long is player already jumping for

    float jumpCount = 0; // maximal possible number of double-jumps

    public bool pressedJump; // update check for spacebar press
    public bool holdingJump;
    public bool isJumping = false;
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
        grounded = IsGrounded();
        Debug.DrawLine(playerTransform.position, playerSpriteTransform.position, Color.green, 2); // just to visualize movement of player
        if (grounded)
        {
            jumpTimeCounter = jumpTime;
        }
        SpaceBarPressed();
        holdingJump = SpaceBarHold();

        CeilingHit();

        time += Time.deltaTime;

        fps = Application.targetFrameRate;

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
        if (Input.GetKeyDown(KeyCode.Space) && grounded){
            pressedJump = true;
        }
    }

    private bool SpaceBarHold()
    {
        return Input.GetKey(KeyCode.Space);
    }

    private void FixedUpdate()
    {
        Move(); // movement to left and right
        Jump();
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


    private void Jump()
    {
        //working jump? WORKING JUMP YAYYYYYYYYYYYYYYYYYYYY

        
        var velocity = playerBody.linearVelocity;
        
 
        if(pressedJump && grounded)
        {
            isJumping = true;
            pressedJump = false;
        }

        if (holdingJump)
        {
            if (jumpTimeCounter > 0 && isJumping)
            {
                //playerBody.velocity = Vector2.up * jumpForce;
                velocity.y = jumpForce;
                playerBody.linearVelocity = velocity;
                jumpTimeCounter -= Time.deltaTime;

            }
            else
            {
                isJumping = false;
            }
        }

        if (!holdingJump)
        {
            isJumping = false;
        }

        

        //Debug.Log("Je ve skoku" + isJumping + " " + jumpTimeCounter);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheck.position, feet.size * 0.85f);
        Gizmos.DrawWireCube(ceilingCheck.position, feet.size * 0.85f);
    }

}
