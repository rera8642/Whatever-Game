using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMain : MonoBehaviour
{
    [SerializeField] private MainArm rocketLauncher;

    [SerializeField] private int currentSelected = 0;
    [SerializeField] private int maxSelected = 1;
    [SerializeField] private int total = 6;
    //[SerializeField] private KeyCode swapKey = KeyCode.Q;

    private MainArm[] objectList;
    private bool allowInvoke = true;

    //public KeyCode debugKey = KeyCode.Z;
    //public KeyCode debugKey2 = KeyCode.X;

    private void Start()
    {
        objectList = new MainArm[total];
        objectList[0] = rocketLauncher;
        currentSelected = 0;
        for (int i = 0; i < maxSelected; i++)
        {
            objectList[i].gameObject.SetActive(false);
        }
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
        else*/
        if (Input.mouseScrollDelta.y < 0 && allowInvoke)
        {
            if (maxSelected > 1)
            {
                allowInvoke = false;
                currentSelected--;
                if (currentSelected < 0)
                {
                    currentSelected = maxSelected - 1;
                }

                Invoke("setAct", 0.2f);
            }
        }
        else if (Input.mouseScrollDelta.y < 0 && allowInvoke)
        {
            if (maxSelected > 1)
            {
                allowInvoke = false;
                currentSelected++;
                if (currentSelected >= maxSelected)
                {
                    currentSelected = 0;
                }

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
                objectList[maxSelected] = rocketLauncher;
                maxSelected++;
                break;
            case 2:
                break;
            case 3:
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
