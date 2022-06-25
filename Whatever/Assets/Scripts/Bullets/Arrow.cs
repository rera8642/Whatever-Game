using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
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
    private Quaternion[] lastRot;
    private int posC;
    private int rotC;
    private bool connected = false;

    private void Start()
    {
        lastPos = new Vector3[1];
        lastPos[0] = transform.position;
        posC = 0;
        initRot = transform.rotation;
        timer = maxTime;
        Setup();
    }

    private void Update()
    {

        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0f && alive)
        {
            Explode();
            alive = false;
        }
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
            if (posC == 1)
            {
                posC = 0;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (allowInvoke)
        {
            /*
            connected = true;
            RaycastHit hit;
            Vector3 dir = (collision.GetContact(0).point - lastPos[posC]).normalized;
            //Vector3 end = transform.position + transform.forward * 2f;
            if (Physics.Raycast(lastPos[posC], dir, out hit, 10f, reactMask))
            {

                Physics.IgnoreCollision(coll, collision.collider);
                //Instantiate(sph, hit.point, Quaternion.identity);
                vis = Instantiate(visual);
                vis.transform.SetParent(null);
                vis.transform.position = hit.point;
                vis.transform.forward = transform.forward;
                vis.GetComponent<Rigidbody>().useGravity = false;
                Physics.IgnoreCollision(vis.GetComponent<BoxCollider>(), collision.collider);
                Vector3 endDir = (hit.point - lastPos[posC]).normalized;
                vis.transform.rotation = initRot;
                if (hit.collider.gameObject.GetComponent<Rigidbody>())
                {
                    vis.GetComponent<AxeVisual>().connectedRB = hit.collider.gameObject.GetComponent<Rigidbody>();
                } else
                {
                    vis.GetComponent<Rigidbody>().detectCollisions = false;
                }
                vis.GetComponent<AxeVisual>().endPos = hit.point;
                Invoke("Delay", .2f);
                allowInvoke = false;
                Destroy(gameObject);
            }
            else
            {
                connected = false;
                Debug.Log("Bruh");
            }*/
            if ((reactMask.value & 1 << collision.gameObject.layer) != 0)
            {
                vis = Instantiate(visual);
                vis.transform.position = collision.GetContact(0).point;
                //vis.transform.forward = transform.forward;
                vis.transform.rotation = initRot;
                if (collision.gameObject.GetComponent<Rigidbody>())
                {
                    vis.GetComponent<AxeVisual>().connectedRB = collision.gameObject.GetComponent<Rigidbody>();
                }
                else
                {
                    vis.GetComponent<Rigidbody>().detectCollisions = false;
                }
                if (collision.gameObject.GetComponent<BasicEnemy>())
                {
                    collision.gameObject.GetComponent<BasicEnemy>().Kill();
                }
                Invoke("Delay", .2f);
                allowInvoke = false;
                Destroy(gameObject);
            }

        }
        /*
        if(allowInvoke)
        {
            vis = Instantiate(visual);
            vis.transform.SetParent(null);
            vis.transform.position = transform.position;
            vis.transform.forward = transform.forward;
            vis.GetComponent<Rigidbody>().useGravity = false;
            Physics.IgnoreCollision(vis.GetComponent<BoxCollider>(), collision.collider);
            if (collision.collider.gameObject.GetComponent<Rigidbody>())
            {
                vis.GetComponent<AxeVisual>().connectedRB = collision.collider.gameObject.GetComponent<Rigidbody>();
            }
            Invoke("Delay", .2f);
            allowInvoke = false;
            Destroy(gameObject);
        }*/
        
    }
    /*
    private void OnCollisionExit(Collision collision)
    {
        if (alive)
        {
            Explode();
            alive = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (alive)
        {
            Explode();
            alive = false;
        }
    }*/

    private void Explode()
    {
        vis = Instantiate(visual, transform.position, Quaternion.identity);
        vis.transform.rotation = transform.rotation;
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
        /*Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, enemyMask);

        for (int i = 0; i < enemies.Length; i++)
        {
            // Take Damage

            if (enemies[i].GetComponent<Rigidbody>())
            {
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);
            }
        }*/


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
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;

        coll.material = physics_mat;

        /*
        rb.AddTorque(transform.right * 30);
        rb.useGravity = useGravity;*/
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
