using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GrapplingGun : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private GameObject img;
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private float minDistance = 1f;

    public LayerMask grappleSurface;
    public Transform gunTip;
    public Transform cam;
    public Transform player;

    private Vector3 grapplePoint;
    public SpringJoint joint;
    public TextMeshProUGUI ammoDisplay;


    private void Update()
    {
        if (Physics.Raycast(cam.position, cam.forward, maxDistance))
        {
            img.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
        } else
        {
            img.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        if (Input.GetMouseButtonDown(1))
        {
            StartGrapple();
        } else if (Input.GetMouseButtonUp(1))
        {
            StopGrapple();
        } else if (!Input.GetMouseButton(1))
        {
            StopGrapple();
        }
        if (ammoDisplay != null)
        {
            ammoDisplay.SetText("∞ / ∞");
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
        if (joint != null)
        {
            Destroy(joint);
        }
    }

    public void SetWhite()
    {
        img.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
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
