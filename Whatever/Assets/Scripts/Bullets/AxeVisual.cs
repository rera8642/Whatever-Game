using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeVisual : MonoBehaviour
{
    public Vector3 endPos;
    public Rigidbody connectedRB;
    public FixedJoint joint;
    public Rigidbody rb;
    private float point;
    private bool allowInvoke = true;
    public float maxLifetime = 10;
    private bool alive = true;
    private bool awake = false;


    void Start()
    {
        //StartCoroutine(LerpPosition(endPos, .1f));
    }
    private void Update()
    {
        if (!awake)
        {
            if (connectedRB)
            {
                joint.connectedBody = connectedRB;
            } else
            {
                Destroy(joint);
                Destroy(rb);
            }
        }
        awake = true;
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0f && alive)
        {
            if (allowInvoke)
            {
                Invoke("Delay", .2f);
                allowInvoke = false;
            }
            alive = false;
        }
    }
    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }
    private void Delay()
    {
        Destroy(gameObject);
    }

}
