using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private float minDistance = 1f;

    public LayerMask grappleSurface;
    public Transform gunTip;
    public Transform cam;
    public Transform player;

    private Vector3 grapplePoint;
    private SpringJoint joint;

    private void Update()
    {
        

        if (Input.GetMouseButtonDown(1))
        {
            StartGrapple();
        } else if (Input.GetMouseButtonUp(1))
        {
            StopGrapple();
        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxDistance))
        {
            float distancefromPoint = Vector3.Distance(player.position, hit.point);
            if (distancefromPoint < minDistance)
            {
                return;
            }
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            joint.maxDistance = distancefromPoint * 0.8f;
            joint.minDistance = distancefromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 5f;
            joint.massScale = 4.5f;

            lineRenderer.positionCount = 2;
        }
    }

    void DrawRope()
    {
        if (!joint)
        {
            return;
        }
        lineRenderer.SetPosition(0, gunTip.position);
        lineRenderer.SetPosition(1, grapplePoint);
    }

    public void StopGrapple()
    {
        lineRenderer.positionCount = 0;
        Destroy(joint);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
