using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Movement : MonoBehaviour {

    public CharacterController2D controller;
    public Animator animator;

        public float runSpeed = 40f;

        public float horizontalMove = 0f;

        bool jump = false;

        bool isTouchingFront;
        bool isTouchingWall;
        public Transform frontCheck;
        bool wallSliding;
        public float wallSlidingSpeed;
        public LayerMask whatIsGrabbable;

        bool wallJumping;
        public float xWallForce;
        public float yWallForce;
        public float wallJumpTime;
        
       
        public float slideSpeed = 800f;

    // Update is called once per frame
    void Update()
    {

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        // Skok
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            RestartLevel();
        }

        // WallSlide operácie
        isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, controller.k_GroundedRadius, whatIsGrabbable);
        isTouchingWall = Physics2D.OverlapCircle(frontCheck.position, controller.k_GroundedRadius, controller.m_WhatIsGround);


        if (isTouchingFront == true && controller.m_Grounded == false && horizontalMove != 0)
        {
            wallSliding = true;
            animator.SetBool("IsSliding", true);
            animator.SetBool("IsJumping", false);
        }
        else
        {
            wallSliding = false;
            animator.SetBool("IsSliding", false);
        }

        if (isTouchingWall == true && controller.m_Grounded == false && horizontalMove != 0)
        {
            animator.SetBool("IsSliding", true);
            animator.SetBool("IsJumping", false);
        }
        else
        {
            animator.SetBool("IsSliding", false);
        }

        if (wallSliding)
        {
            controller.m_Rigidbody2D.velocity = new Vector2(controller.m_Rigidbody2D.velocity.x, Mathf.Clamp(controller.m_Rigidbody2D.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }

        if (Input.GetButtonDown("Jump") && wallSliding == true)
        {
            //jump = true;
            wallJumping = true;

            animator.SetBool("IsSliding", false);
            Invoke("SetWallJumpingToFalse", wallJumpTime);
        }

        if (wallJumping == true)
        {
            controller.m_Rigidbody2D.velocity = new Vector2(xWallForce * -horizontalMove, yWallForce);
            animator.SetBool("IsJumping", true);
        }



        if (Input.GetButtonDown("Fire1") && controller.m_Grounded == true)
        {
            performSlide();
        }
    }
    private void performSlide()
    {     
        animator.SetBool("IsGroundSliding", true);
        if (controller.m_FacingRight)
        {
            controller.m_Rigidbody2D.AddForce(Vector2.right * slideSpeed);
        } 
        else
        {
            controller.m_Rigidbody2D.AddForce(Vector2.left * slideSpeed);
        }
        StartCoroutine("stopSlide");
    }

    IEnumerator stopSlide()
    {
        yield return new WaitForSeconds(0.8f);
        animator.Play("Idle");
        animator.SetBool("IsGroundSliding", false);       
    }

    public void OnLanding ()
    {
            animator.SetBool("IsJumping", false);       
    } 

    void FixedUpdate () {
        //pohyb Playera
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
       

    }
    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void SetWallJumpingToFalse()
    {
        wallJumping = false;
    }
}
