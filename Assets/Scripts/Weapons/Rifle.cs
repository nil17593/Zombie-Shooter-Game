using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    protected override void Start()
    {
        base.Start();  // Initialize base class variables
    }

    public override void Fire()
    {
        if (!CanFire()) return;

        lastShotTime = Time.time;  // Update shot time

        // Logic to shoot a bullet (single shot)
        ShootBullet();

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
        }
    }
}
