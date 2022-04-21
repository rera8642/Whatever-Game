using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlameThrower : MonoBehaviour
{
    [SerializeField] private GameObject flameEffect;
    [SerializeField] private Transform tip;
    public TextMeshProUGUI ammoDisplay;

    private GameObject effect = null;
    [SerializeField] private int maxAmmo = 300;
    private int ammo = 300;
    private float ammoLoss = 0;
    private bool reloading = false;
    private float reloadTime = 3f;
    // Update is called once per frame
    private void Start()
    {
        ammo = maxAmmo;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (ammo > 0)
            {
                StartFlameThrower();
            }
        } else if (!Input.GetMouseButton(1))
        {
            EndFlameThrower();
        }

        if (effect != null)
        {
            ammoLoss += .125f;
            ammo = maxAmmo - (int)ammoLoss;
            if (ammo <= 0)
            {
                ammo = 0;
                ammoLoss = maxAmmo;
                EndFlameThrower();
            }
        } else
        {
            if (Input.GetKeyDown(KeyCode.R) && ammo < maxAmmo && !reloading)
            {
                Reload();
            }
        }
        if (ammoDisplay != null)
        {
            ammoDisplay.SetText(ammo + " / " + maxAmmo);
        }
        

    }
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        ammo = maxAmmo;
        ammoLoss = 0;
        reloading = false;
    }
    private void StartFlameThrower()
    {
        effect = Instantiate(flameEffect, tip);
        effect.transform.parent = transform;
        ammoLoss += 10;

    }
    public void EndFlameThrower()
    {
        if (effect != null)
        {
            Destroy(effect);
            effect = null;
        }
    }

}
