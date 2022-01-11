using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyMovement : MonoBehaviour
{
    private float horizontalMovement;
    private float verticalMovement;
    private float movementMultiplier = 10f;
    private float airMultiplier = .9f;
    private Vector3 movementDirection;
    public bool grounded = false;

    [SerializeField] private LayerMask FloorMask;
    [SerializeField] private Transform FeetTransform;
    [SerializeField] private Rigidbody PlayerBody;
    [Space]
    [SerializeField] private float speed;
    [SerializeField] private float airSpeed;
    [SerializeField] private float groundDrag;
    [SerializeField] private float airDrag;
    [SerializeField] private float jumpForce;

    private void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
        movementDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;
        grounded = Physics.CheckSphere(FeetTransform.position, 0.1f, FloorMask);

        ControlDrag();

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    { 
        MovePlayer();
    }

    private void MovePlayer()
    {
        float spd = airSpeed * airMultiplier;
        if (grounded)
        {
            spd = speed * movementMultiplier;
        }
        PlayerBody.AddForce(movementDirection.normalized * spd, ForceMode.Acceleration);
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
        PlayerBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
}
