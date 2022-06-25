using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyMovement : MonoBehaviour
{ 
    [SerializeField] private LayerMask FloorMask;
    [SerializeField] private Transform FeetTransform;
    [SerializeField] private Transform orientation;
    [SerializeField] private Rigidbody PlayerBody;
    [SerializeField] private WallRunning wallRun;
    [SerializeField] private GrapplingGun grappleGun;
    [Space]
    [SerializeField] private float speed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float wallRunSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float airSpeed;
    [SerializeField] private float groundDrag;
    [SerializeField] private float airDrag;
    [SerializeField] private float jumpForce;
    [SerializeField] private float grappleJumpForce;
    [SerializeField] private float grappleForwardForce;
    [Space]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

    private float horizontalMovement;
    private float verticalMovement;
    private float movementMultiplier = 10f;
    private float airMultiplier = .9f;
    private float wallMultiplier = 3f;
    private float playerHeight = 2f;

    private float gjForce;
    private float gfForce;

    private Vector3 movementDirection;
    private Vector3 slopeMoveDirection;
    private bool grounded = false;
    private RaycastHit slopeHit;

    private void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
        movementDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
        grounded = Physics.CheckSphere(FeetTransform.position, 0.3f, FloorMask);
        if (grounded)
        {
            gjForce = grappleJumpForce;
            gfForce = grappleForwardForce;
        }

        ControlSpeed();

        ControlDrag();

        if (Input.GetKeyDown(jumpKey))
        {
            if (grappleGun.IsGrappling())
            {
                if (grappleGun.attachedToPhys)
                {
                    grappleGun.GrappleSend(transform.position);
                } else if (grappleGun.attachedToEnemy)
                {
                    grappleGun.GrappleEnemy(transform.position);
                } else 
                {
                    GrappleJump();
                    grappleGun.StopGrapple();
                }
            } else if (grounded)
            {
                Jump();
            }
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(movementDirection, slopeHit.normal);
    }

    private void FixedUpdate()
    { 
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 vec = movementDirection.normalized * speed * airMultiplier;
        if (grounded)
        {
            vec = movementDirection.normalized * speed * movementMultiplier;
            if (OnSlope())
            {
                vec = slopeMoveDirection.normalized * speed * movementMultiplier;
            }
        } else
        {
            if (wallRun.isWallRun)
            {
                vec = slopeMoveDirection.normalized * speed * wallMultiplier;
            }
        }
        PlayerBody.AddForce(vec, ForceMode.Acceleration);
    }

    private void ControlSpeed()
    {
        if (Input.GetKey(sprintKey) && grounded)
        {
            speed = Mathf.Lerp(speed, sprintSpeed, acceleration * Time.deltaTime);
        } else if (wallRun.isWallRun)
        {
            speed = Mathf.Lerp(speed, wallRunSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            speed = Mathf.Lerp(speed, walkSpeed, acceleration * Time.deltaTime);
        }
    }

    private void ControlDrag()
    {
        if (grounded)
        {
            PlayerBody.drag = groundDrag;
        } else
        {
            PlayerBody.drag = airDrag;
        }
    }

    private void Jump()
    {
        PlayerBody.velocity = new Vector3(PlayerBody.velocity.x, 0f, PlayerBody.velocity.z);
        PlayerBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void GrappleJump()
    {
        if (PlayerBody.velocity.y < 0f)
        {
            PlayerBody.velocity = new Vector3(PlayerBody.velocity.x, 0f, PlayerBody.velocity.z);
        }
        PlayerBody.AddForce(transform.up * gjForce + orientation.forward * gfForce, ForceMode.Impulse);
        gjForce *= .95f;
        gfForce *= .95f;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
        }
        return false;
    }
}
