using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGrappleGun : MonoBehaviour
{
    public GrapplingGun grappleGun;

    private Quaternion desiredRotation;
    public float rotationSpeed = 5f;

    private void Update()
    {
        if (!grappleGun.IsGrappling())
        {
            desiredRotation = transform.parent.rotation;
        } else
        {
            desiredRotation = Quaternion.LookRotation(grappleGun.GetGrapplePoint() - transform.position);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
    }
}
