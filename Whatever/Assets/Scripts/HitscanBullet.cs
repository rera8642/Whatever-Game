using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitscanBullet : MonoBehaviour
{
    public GameObject sphere;
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammoDisplay;

    public Camera fpsCam;
    public CameraController cam;
    public Transform attackPoint;
    public Transform orientation;

    public Rigidbody playerBody;
    public LayerMask enemyMask;
    public LayerMask physMask;
    public float recoilForce;

    public bool allowInvoke = true;
    public bool knockback = false;
    public float explosionForce = 0;
    public float explosionRange = 0;
    public float range;

    public float shootForce;
    public float verticalForce;

    public float timeBetweenShooting;
    public float spread;
    public float reloadTime;
    public float timeBetweenShots;

    public int magSize;
    public int bulletsPerTap;

    public bool allowButtonHold;
    public int mouseButton = 0;
    public bool allowReload = true;

    private int bulletsLeft;
    private int bulletsShot;

    private bool shooting;
    private bool readyToShoot;
    private bool reloading;

    private void Awake()
    {
        bulletsLeft = magSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

        if (ammoDisplay != null)
        {
            ammoDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magSize / bulletsPerTap);
        }
    }
    private void MyInput()
    {
        if (allowButtonHold)
        {
            shooting = Input.GetMouseButton(mouseButton);
        }
        else
        {
            shooting = Input.GetMouseButtonDown(mouseButton);
        }
        if (allowReload)
        {
            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magSize && !reloading)
            {
                Reload();
            }
        }
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
        {
            Reload();
        }
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;
        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, physMask)) {
            if (knockback)
            {
                Instantiate(sphere, hit.point, Quaternion.identity);
                hit.rigidbody.AddExplosionForce(explosionForce, hit.point, explosionRange);
                if (hit.collider.gameObject.GetComponent<BasicEnemy>())
                {
                    hit.collider.gameObject.GetComponent<BasicEnemy>().Kill();
                }
            }
            
        }
        if (muzzleFlash != null)
        {
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        }

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
            playerBody.AddForce(-fpsCam.transform.forward.normalized * recoilForce, ForceMode.Impulse);
            cam.recoil = -1;
            cam.recoilTime = .5f;
            cam.minRecoilTime = 0;
        }

        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magSize;
        reloading = false;
    }

}
