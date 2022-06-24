using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GrapplingGun : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private GameObject img;
    [SerializeField] private LayerMask physMask;

    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float sendForce = 10f;


    public LayerMask grappleSurface;
    public Transform gunTip;
    public Transform cam;
    public Transform player;

    private Vector3 grapplePoint;
    public SpringJoint joint;
    public TextMeshProUGUI ammoDisplay;
    public bool attachedToPhys = false;
    private Rigidbody connectedBody;
    private float dist;

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
            dist = distancefromPoint;
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
            if (physMask == (physMask | (1 << hit.collider.gameObject.layer)))
            {
                attachedToPhys = true;
                Rigidbody crb = hit.collider.gameObject.GetComponent<Rigidbody>();
                if (crb != null)
                {
                    connectedBody = crb;
                }
            }
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
    public void GrappleSend(Vector3 pos)
    {
        if (connectedBody != null)
        {
            Vector3 dir = (pos - connectedBody.transform.position).normalized;
            connectedBody.AddForce(dir * sendForce * dist/10, ForceMode.Impulse);
        }
        StopGrapple();
    }
    public void StopGrapple()
    {
        lineRenderer.positionCount = 0;
        if (joint != null)
        {
            Destroy(joint);
        }
        attachedToPhys = false;
        connectedBody = null;
        dist = 0;
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
