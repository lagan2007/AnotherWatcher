using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
//using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

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
    [SerializeField]
    private LayerMask Invincible;
    [SerializeField]
    private LayerMask Default;
    [SerializeField]
    KeyCode jumpButton;
    [SerializeField]
    KeyCode dashButton;
    [SerializeField]
    float maxCoyotTime;
    [SerializeField]
    float maxDashCooldown;
    [SerializeField]
    SpriteRenderer spriteRenderer;
    [SerializeField]
    CinemachineVirtualCamera virtualCamera;
    [SerializeField]
    public ParticleSystem hitParticles;
    [SerializeField]
    PlayerHp playerHp;
    [SerializeField]
    Save save;

    public CinemachineBasicMultiChannelPerlin virtualCameraPerlin;

    public float jumpTimeCounter; // how long is player already jumping for

    public float dashTimeCounter;

    public bool canMove;
    public float canMoveoCoolDown;
    public bool pressedJump; // update check for spacebar press
    public bool holdingJump; // update check for spacebar hold
    public bool releasedJump;
    public bool pressedDash;

    public bool hasIFrames = false;
    public bool dashObtained;
    
    public float dashCooldown;
    public float coyotTime;
    public bool isDashing = false;
    public bool isJumping;
    public bool isDoingDouble;
    public bool grounded; // is player on ground?
    public bool hasRunOnGrounded;

    public float fps;
    public float time;

    float finalHorizontalInput;

    float calculatedMove;


    bool hasRunOnDash = false;
    private float fallSpeedYDdampingTreshold;

    public bool playerIsOnRight;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = fpsSet;
        fallSpeedYDdampingTreshold = CameraManager.instance.fallSpeedDampingChangeTreshold;
        virtualCameraPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Save"))
        {
            save.SaveData();
        }

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
        if (dashObtained)
        {
            DashKeyPressed();
        }
        

      
        CeilingHit();

        time += Time.deltaTime; //session time

        fps = Application.targetFrameRate; // fps value (pre set)

        
        ManageCameras();
        
    }

    float knockbackTime = 0.2f;

    public IEnumerator Knockback()
    {
        if (playerHp.currentHp > 0)
        {
            hasIFrames = true;
            this.gameObject.layer = LayerMask.NameToLayer("Invincible");
        }



        float kTimer = 0;

        canMove = false;

        while (kTimer <= knockbackTime)
        {
            if (playerIsOnRight)
            {
                playerBody.linearVelocity = new Vector2(15, 15);
            }
            else
            {
                playerBody.linearVelocity = new Vector2(-15, 15);
            }





            virtualCameraPerlin.m_AmplitudeGain = 5;
            kTimer = kTimer + Time.deltaTime;
            yield return null;
        }

        if (playerHp.currentHp > 0)
        {
            StartCoroutine(flicker());
        }


        yield return new WaitForSeconds(0.2f);
        virtualCameraPerlin.m_AmplitudeGain = 0;
        canMove = true;



        yield return new WaitForSeconds(1.8f);

        hasIFrames = false;
        this.gameObject.layer = LayerMask.NameToLayer("Default");


        yield return null;
    }

    public IEnumerator flicker()
    {
        while (hasIFrames)
        {
            spriteRenderer.sortingOrder = -100;
            spriteRenderer.sortingLayerName = "BackGround";
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.sortingOrder = 1;
            spriteRenderer.sortingLayerName = "Default";
            yield return new WaitForSeconds(0.1f);
            yield return null;
        }
        
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
        bool hitCeling = Physics2D.OverlapBox(ceilingCheck.position, feet.size * 0.95f, 0f, WhatIsGround);

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
        return Physics2D.OverlapBox(groundCheck.position, feet.size * 0.95f, 0f, WhatIsGround);
    }

    private void SpaceBarPressed()
    {
        if (Input.GetKeyDown(jumpButton) && (grounded || jumpCount > 0)){
            pressedJump = true;

        }
    }

    private void JumpKeyReleased()
    {
        if (Input.GetKeyUp(jumpButton))
        {
            releasedJump = true;
        }
    }

    private void DashKeyPressed()
    {
        if (Input.GetKeyDown(dashButton))
        {
            pressedDash = true;
 
        }

        
    }

    private bool SpaceBarHold()
    {
        if(isJumping)
        {
            return Input.GetKey(jumpButton);
        }
        else
        {
            return false;
        }
    }

    private void FixedUpdate()
    {

        GroundedManagment();

        canMoveoCoolDown -= Time.deltaTime;


        if (canMove)
        {
            if (!isDashing)
            {
                //Move(); // movement to left and right
                Jump();
            }
            if (dashObtained)
            {
                Dash();
            }
            
        }
        else
        {
            calculatedMove = 0;
        }

        if (!isDashing)
        {
            Move();
        }

       //Debug.Log(calculatedMove);
       

        if (canMoveoCoolDown >= 0)
        {
            playerBody.linearVelocity = new Vector2(0,0);
        }
    }


    private void GroundedManagment()
    {
        if (IsGrounded())
        {
            jumpTimeCounter = jumpTime;
            coyotTime = maxCoyotTime;
            animator.SetBool("Grounded", true);
            if (!hasRunOnGrounded)
            {
                isJumping = false;
                hasRunOnGrounded = true;
            }
        }

        if (!IsGrounded())
        {
            coyotTime -=Time.deltaTime;
            if (!isDashing)
            {
                playerBody.gravityScale = 10;
                animator.SetBool("Grounded", false);
            }
            hasRunOnGrounded = false;
        }

        if(coyotTime >= 0)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        if (grounded)
        {

            
            if (IsGrounded() || isDashing)
            {
                playerBody.gravityScale = 0;
            }
            
            jumpCount = 1;
            if (!isDashing)
            {
                dashTimeCounter = dashTime;
            }
        }
    }

    private void Move()
    {
        //get imput for moving right and left
       
        if (canMove)
        {
            calculatedMove = Input.GetAxisRaw("Horizontal") * playerSpeed;

            //rotate player based on input
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                playerTransform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                playerTransform.rotation = Quaternion.Euler(0, 0, 0);
            }

            playerBody.linearVelocity = new Vector2(calculatedMove, playerBody.linearVelocity.y);
        }
           
        






    }

    private void Dash()
    {
        float inputVertical = Input.GetAxisRaw("Vertical");
        float inputHorizontal = Input.GetAxisRaw("Horizontal");



        if (inputHorizontal != 0 || inputVertical != 0)
        {
            finalHorizontalInput = inputHorizontal;
        }



        if (pressedDash && (dashCooldown <= 0))
        {
            isDashing = true;
            isJumping = false;
        }

        dashCooldown -= Time.deltaTime;

        if(isDashing && dashTimeCounter > 0)
        {
            if (gameObject.layer != LayerMask.NameToLayer("Invincible"))
            {
                gameObject.layer = LayerMask.NameToLayer("Invincible");
            }
            playerBody.linearVelocity = new Vector2 (dashStrenght * finalHorizontalInput, dashStrenght * inputVertical);
            dashTimeCounter -= Time.deltaTime;
            playerBody.gravityScale = 0;
            hasRunOnDash = false;
        }else
        {
            if(!hasRunOnDash)
            {
                if (IsGrounded())
                {
                    dashCooldown = maxDashCooldown;
                }

                if (inputVertical != -1)
                {
                    var playerVelocity = playerBody.linearVelocity;
                    playerVelocity.y = 0;
                    playerBody.linearVelocity = playerVelocity;
                    hasRunOnDash = true;
                }
            }
            

            if(gameObject.layer != LayerMask.NameToLayer("Default") && !hasIFrames)
            {
                gameObject.layer = LayerMask.NameToLayer("Default");
            }
                
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
        Gizmos.DrawWireCube(groundCheck.position, feet.size * 0.95f);
        Gizmos.DrawWireCube(ceilingCheck.position, feet.size * 0.95f);
    }

}