using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceGrenade : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject explosion;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private LayerMask reactMask;

    [Range(0f, 1f)] [SerializeField] private float bounciness;
    [SerializeField] private bool useGravity;
    [SerializeField] private float explosionRange;

    [SerializeField] private float maxLifetime;



    public bool explodeOnTouch = true;
    public int explosionDamage;
    public float explosionForce;

    private bool allowInvoke = true;
    private bool alive = true;
    PhysicMaterial physics_mat;

    private void Start()
    {
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*
        Explode();
        alive = false;*/
    }

    private void Explode()
    {
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
        /*
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, enemyMask);

        for (int i = 0; i < enemies.Length; i++)
        {
            // Take Damage

            if (enemies[i].GetComponent<Rigidbody>())
            {
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);
            }
        }
        */
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
