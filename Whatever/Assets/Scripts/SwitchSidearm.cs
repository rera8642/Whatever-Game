using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSidearm : MonoBehaviour
{
    [SerializeField] private GrapplingGun grappleGun;
    [SerializeField] private FlameThrower flameThrower;
    [SerializeField] private Gun iceGreandeLauncher;

    [SerializeField] private int currentSelected = 0;
    [SerializeField] private int maxSelected = 3;
    [SerializeField] private KeyCode swapKey = KeyCode.Q;

    private GameObject[] objectList;
    private bool allowInvoke = true;

    private void Start()
    {
        objectList = new GameObject[maxSelected];
        objectList[0] = grappleGun.gameObject;
        objectList[1] = flameThrower.gameObject;
        objectList[2] = iceGreandeLauncher.gameObject;
        currentSelected = 0;
        foreach (GameObject obj in objectList)
        {
            obj.SetActive(false);
        }
        //iceGreandeLauncher.ammoDisplay.gameObject.SetActive(false);
        objectList[currentSelected].SetActive(true);
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(swapKey) && allowInvoke)
        {
            allowInvoke = false;
            //iceGreandeLauncher.ammoDisplay.gameObject.SetActive(false);
            switch (currentSelected)
            {
                case 0:
                    grappleGun.StopGrapple();
                    //Destroy(grappleGun.joint);
                    break;
                case 1:
                    flameThrower.EndFlameThrower();
                    break;
                case 2:
                    break;
            }
            currentSelected++;

            if (currentSelected >= maxSelected)
            {
                currentSelected = 0;
            }

            Invoke("setAct", 0.2f);
            
        }
    }
    private void setAct()
    {
        foreach (GameObject obj in objectList)
        {
            obj.SetActive(false);
        }
        //iceGreandeLauncher.ammoDisplay.gameObject.SetActive(false);
        objectList[currentSelected].SetActive(true);
        if (currentSelected == 2)
        {
            //iceGreandeLauncher.ammoDisplay.gameObject.SetActive(true);
        }
        allowInvoke = true;
    }
}
