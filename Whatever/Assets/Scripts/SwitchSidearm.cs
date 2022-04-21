using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSidearm : MonoBehaviour
{
    [SerializeField] private Sidearm grappleGun;
    [SerializeField] private Sidearm flameThrower;
    [SerializeField] private Sidearm iceGreandeLauncher;
    [SerializeField] private Sidearm handCannon;
    [SerializeField] private Sidearm axe;

    [SerializeField] private int currentSelected = 0;
    [SerializeField] private int maxSelected = 5;
    [SerializeField] private int total = 6;
    [SerializeField] private KeyCode swapKey = KeyCode.Q;

    private Sidearm[] objectList;
    private bool allowInvoke = true;

    //public KeyCode debugKey = KeyCode.Z;
    //public KeyCode debugKey2 = KeyCode.X;

    private void Start()
    {
        objectList = new Sidearm[total];
        objectList[0] = grappleGun;
        objectList[1] = flameThrower;
        objectList[2] = iceGreandeLauncher;
        objectList[3] = handCannon;
        objectList[4] = axe;
        currentSelected = 0;
        for(int i = 0; i < maxSelected; i++)
        {
            objectList[i].gameObject.SetActive(false);
        }
        //iceGreandeLauncher.ammoDisplay.gameObject.SetActive(false);
        objectList[currentSelected].gameObject.SetActive(true);
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(debugKey) && allowInvoke)
        {
            allowInvoke = false;
            unlockWeapon(2);
            Invoke("setAct", 0.2f);
        } 
        else if (Input.GetKeyDown(debugKey2) && allowInvoke)
        {
            allowInvoke = false;
            unlockWeapon(3);
            Invoke("setAct", 0.2f);
        } 
        else*/ if (Input.GetKeyDown(swapKey) && allowInvoke)
        {
            if (maxSelected > 1)
            {
                allowInvoke = false;
                //iceGreandeLauncher.ammoDisplay.gameObject.SetActive(false);
                objectList[currentSelected].switchOff();
                currentSelected++;
                for (int i = 0; i < maxSelected; i++)
                {
                    objectList[i].gameObject.SetActive(false);
                }
                
                if (currentSelected >= maxSelected)
                {
                    currentSelected = 0;
                }
                objectList[currentSelected].gameObject.SetActive(true);

                Invoke("setAct", 0.2f);
            }
        }
    }
    private void setAct()
    {
        
        for (int i = 0; i < maxSelected; i++)
        {
            objectList[i].gameObject.SetActive(false);
        }
        objectList[currentSelected].gameObject.SetActive(true);
        allowInvoke = true;
    }

    public void noSidearm()
    {
        for (int i = 0; i < maxSelected; i++)
        {
            objectList[i].gameObject.SetActive(false);
        }
        currentSelected = maxSelected;
    }
    public void unlockWeapon(int type)
    {
        if (maxSelected >= total)
        {
            return;
        }
        switch (type)
        {
            case 1:
                objectList[maxSelected] = grappleGun;
                maxSelected++;
                break;
            case 2:
                objectList[maxSelected] = flameThrower;
                maxSelected++;
                break;
            case 3:
                objectList[maxSelected] = iceGreandeLauncher;
                maxSelected++;
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            default:
                return;
        }
    }
}
