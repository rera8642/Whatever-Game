using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject explosion;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private LayerMask physMask;
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

    private void Start()
    {
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

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (alive)
        {
            Explode();
            alive = false;
        }
    }
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
    }

    private void Explode()
    {
        Instantiate(point, transform.position, Quaternion.identity);
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, enemyMask);
        Collider[] reactors  = Physics.OverlapSphere(transform.position, explosionRange, physMask);
        for (int i = 0; i < enemies.Length; i++)
        {
            // Take Damage
            enemies[i].gameObject.GetComponent<BasicEnemy>().Kill();

        }
        for (int i = 0; i < reactors.Length; i++)
        {
            // Take Damage

            if (reactors[i].GetComponent<Rigidbody>())
            {
                reactors[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);
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
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;

        GetComponent<SphereCollider>().material = physics_mat;

        rb.useGravity = useGravity;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
