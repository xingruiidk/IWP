using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public static Movement instance;
    public float moveSpeed = 5.0f;
    private Animator animator;
    public bool isGrounded;
    public LayerMask floorMask;
    public Rigidbody rb;
    public float jumpForce;
    private float gravity = -10f;
    private Vector3 moveVel;

    void Start()
    {
        instance = this;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 
    }

    void Update()
    {
        HandleMovement();
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
        CheckGrounded();
        animationCheck();
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = (CameraThing.instance.NSEW.forward * verticalInput + CameraThing.instance.transform.right * horizontalInput).normalized;

        if (isGrounded)
        {
            moveVel = moveDirection * moveSpeed;
            rb.velocity = new Vector3(moveVel.x, rb.velocity.y, moveVel.z);

            if (moveDirection.magnitude > 0)
            {
                animator.SetFloat("WASD", 1);
                transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * 20f); 
            }
            else
            {
                animator.SetFloat("WASD", 0);
            }
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void Jump()
    {
        animator.SetTrigger("Jump");
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }

    public void CheckGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 1.2f, floorMask))
        {
            isGrounded = true;
            Debug.DrawRay(transform.position + Vector3.up, Vector3.down * 1.2f, Color.blue);
        }
        else
        {
            isGrounded = false;
            Debug.DrawRay(transform.position + Vector3.up, Vector3.down * 1.2f, Color.red);
        }
    }
    public void animationCheck()
    {
        animator.SetBool("InAir", !isGrounded);
        animator.SetBool("FromGroundAttack", isGrounded);
    }
}
