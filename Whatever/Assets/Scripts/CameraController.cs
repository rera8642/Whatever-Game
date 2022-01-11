using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;
    [Space]
    [SerializeField] private Camera mainCam;

    private float mouseX;
    private float mouseY;

    private float multiplier = 0.1f;

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

        mainCam.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);

        transform.rotation = Quaternion.Euler(0f, yRot, 0f);
    }
}
