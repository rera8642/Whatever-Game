using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainArm : MonoBehaviour
{
    [SerializeField] private Gun rocketLauncher;
    public int type = 1;

    public void switchOff()
    {
        switch (type)
        {
            case 1:
                if (!rocketLauncher)
                {
                    return;
                }
                
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
