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

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StartFlameThrower();
        } else if (!Input.GetMouseButton(1))
        {
            EndFlameThrower();
        }
        if (ammoDisplay != null)
        {
            ammoDisplay.SetText("∞ / ∞");
        }

    }

    private void StartFlameThrower()
    {
        effect = Instantiate(flameEffect, tip);
        effect.transform.parent = transform;

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
