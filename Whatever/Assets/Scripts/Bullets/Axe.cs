using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider coll;
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject visual;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private LayerMask reactMask;

    [Range(0f, 1f)] [SerializeField] private float bounciness;
    [SerializeField] private bool useGravity;
    [SerializeField] private float explosionRange;

    [SerializeField] private float maxLifetime;
    [SerializeField] private float torque;

    public GameObject point;

    public bool explodeOnTouch = true;
    public int explosionDamage;
    public float explosionForce;

    private bool allowInvoke = true;
    private bool alive = true;
    PhysicMaterial physics_mat;

    public float maxTime = 10;
    private float timer;
    private GameObject vis;
    private Quaternion initRot;
    private Vector3[] lastPos;
    private int posC;
    private bool connected = false;
    public GameObject sph;

    private void Start()
    {
        lastPos = new Vector3[3];
        lastPos[0] = transform.position;
        lastPos[1] = transform.position;
        lastPos[2] = transform.position;
        posC = 0;
        timer = maxTime;
        Setup();
    }

    private void Update()
    {
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
        /*
        if (connected)
        {

            rb.angularVelocity *= 0.9f;
            if (rb.angularVelocity.magnitude <= .1f)
            {
                rb.angularVelocity = Vector3.zero;
            }
        }*/
        /*
        timer--;
        if (timer <= 0)
        {
            Instantiate(point, transform.position, Quaternion.identity);
            timer = maxTime;
        }*/
        if (!connected)
        {
            lastPos[posC] = transform.position;
            posC += 1;
            if (posC == 3)
            {
                posC = 0;
            }
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        //Physics.IgnoreCollision(coll, collision.collider);

        //transform.SetParent(collision.transform);
        if (allowInvoke)
        {
            connected = true;
            RaycastHit hit;
            Vector3 dir = (transform.position - lastPos[posC]).normalized;
            //Vector3 end = transform.position + transform.forward * 2f;
            
            if (Physics.Raycast(lastPos[posC],dir,out hit, 5f, reactMask))
            {
                
                Physics.IgnoreCollision(coll, collision.collider);
                //Instantiate(sph, hit.point, Quaternion.identity);
                vis = Instantiate(visual);
                vis.transform.SetParent(null);
                vis.transform.position = hit.point;
                vis.transform.forward = transform.forward;
                vis.transform.rotation = initRot;
                if (hit.collider.gameObject.GetComponent<Rigidbody>())
                {
                    vis.GetComponent<AxeVisual>().connectedRB = hit.collider.gameObject.GetComponent<Rigidbody>();
                }
                vis.GetComponent<AxeVisual>().endPos = hit.point;
                Invoke("Delay", .2f);
                allowInvoke = false;
                Destroy(gameObject);
            }else
            {
                connected = false;
            }

        }
    }

    private void Explode()
    {
        //Instantiate(point, transform.position, Quaternion.identity);
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, enemyMask);

        for (int i = 0; i < enemies.Length; i++)
        {
            // Take Damage

            if (enemies[i].GetComponent<Rigidbody>())
            {
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);
            }
        }

        if (allowInvoke)
        {
            Invoke("Delay", 0.05f);
            allowInvoke = false;
        }
    }

    private void Delay()
    {
        Destroy(gameObject);
    }

    private void Setup()
    {
        initRot = transform.rotation;
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;

        GetComponent<SphereCollider>().material = physics_mat;

        rb.useGravity = useGravity;
        float turn = Input.GetAxis("Horizontal");
        rb.AddTorque(transform.right * torque);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
