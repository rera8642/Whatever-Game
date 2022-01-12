using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float sensX = 180f;
    [SerializeField] private float sensY = 180f;
    [Space]
    [SerializeField] private Transform mainCam;
    [SerializeField] private Transform orientation;
    [SerializeField] WallRunning wallRun;

    private float mouseX;
    private float mouseY;

    private float multiplier = 0.01f;

    private float xRot;
    private float yRot;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRot += mouseX * sensX * multiplier;
        xRot -= mouseY * sensY * multiplier;

        xRot = Mathf.Clamp(xRot, -90f, 90f);

        mainCam.transform.rotation = Quaternion.Euler(xRot, yRot, wallRun.tilt);

        orientation.transform.rotation = Quaternion.Euler(0f, yRot, 0f);
    }
}
