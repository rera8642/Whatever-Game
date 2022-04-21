using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sidearm : MonoBehaviour
{
    [SerializeField] private GrapplingGun grappleGun;
    [SerializeField] private FlameThrower flameThrower;
    [SerializeField] private Gun iceGreandeLauncher;
    [SerializeField] private Gun handCannon;
    [SerializeField] private Gun axe;
    public int type = 5;

    public void switchOff()
    {
        switch (type)
        {
            case 1:
                if (!grappleGun)
                {
                    return;
                }
                grappleGun.StopGrapple();
                grappleGun.SetWhite();
                break;
            case 2:
                if (!flameThrower)
                {
                    return;
                }
                flameThrower.EndFlameThrower();
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
