using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{

    public List<Collider> RagdollParts = new List<Collider>();
    public Animator animator;
    public bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        SetRagdollParts();
    }

    private void SetRagdollParts()
    {
        Collider[] colliders = this.gameObject.GetComponentsInChildren<Collider>();
        foreach(Collider c in colliders)
        {
            if (c.gameObject != this.gameObject)
            {
                c.isTrigger = true;
                RagdollParts.Add(c);
            }
        }
    }

    private void SetRagdollOn()
    {
        Collider[] colliders = this.gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider c in colliders)
        {
            if (c.gameObject != this.gameObject)
            {
                c.isTrigger = false;
            } else
            {
                //c.isTrigger = true;
            }
        }
    }

    public void Kill()
    {
        animator.enabled = false;
        SetRagdollOn();
        dead = true;
    }
}
