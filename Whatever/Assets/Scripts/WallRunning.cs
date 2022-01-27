using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [SerializeField] private Transform orientation;

    [SerializeField] private float wallDistance = 0.5f;
    [SerializeField] private float minimumJumpHeight = 1.5f;
    [SerializeField] private float wallRunGrav;
    [SerializeField] private float wallJumpForce;
    [Header("Camera")]
    [SerializeField] private Camera cam;
    [SerializeField] private GrapplingGun grappleGun;
    [SerializeField] private float fov;
    [SerializeField] private float wallRunfov;
    [SerializeField] private float wallRunfovTime;
    [SerializeField] private float camTilt;
    [SerializeField] private float camTiltTime;
    [Space]
    [SerializeField] private Rigidbody PlayerBody;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private LayerMask wallMask;

    public float tilt { get; private set; }
    public bool isWallRun { get; private set; }

    private bool wallLeft = false;
    private bool wallRight = false;

    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;

    private void Update()
    {
        CheckWall();
        //Debug.Log("WallLeft " + wallLeft);
        //Debug.Log("WallRight " + wallRight);

        if (CanWallRun())
        {
            if (wallLeft)
            {
                StartWallRun();
            } else if (wallRight)
            {
                StartWallRun();
            } else
            {
                StopWallRun();
            }
        } else
        {
            StopWallRun();
        }
    }

    private void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance, wallMask);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance, wallMask);
    }

    private bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }

    private void StartWallRun()
    {
        isWallRun = true;
        PlayerBody.useGravity = false;

        PlayerBody.AddForce(Vector3.down * wallRunGrav, ForceMode.Force);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunfov, wallRunfovTime * Time.deltaTime);

        if (wallLeft)
        {
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        } else if (wallRight)
        {
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
        }

        if (Input.GetKeyDown(jumpKey) && !grappleGun.IsGrappling())
        {
            if (wallLeft)
            {
                Vector3 wallRunJumpDir = transform.up + leftWallHit.normal;
                PlayerBody.velocity = new Vector3(PlayerBody.velocity.x, 0, PlayerBody.velocity.z);
                PlayerBody.AddForce(wallRunJumpDir * wallJumpForce * 100f, ForceMode.Force);
            } else if (wallRight)
            {
                Vector3 wallRunJumpDir = transform.up + rightWallHit.normal;
                PlayerBody.velocity = new Vector3(PlayerBody.velocity.x, 0, PlayerBody.velocity.z);
                PlayerBody.AddForce(wallRunJumpDir * wallJumpForce * 100f, ForceMode.Force);
            }
        }
    }

    private void StopWallRun()
    {
        isWallRun = false;
        PlayerBody.useGravity = true;

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunfovTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }
}
