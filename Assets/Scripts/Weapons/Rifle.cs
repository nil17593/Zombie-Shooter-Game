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
        currentAmmo--;

        // Logic to shoot a bullet (single shot)
        ShootBullet();

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
        }
    }

    //private new void ShootBullet()
    //{
    //    Bullet bullet = objectPool.GetPooledObject();
    //    if (bullet != null)
    //    {
    //        bullet.transform.position = bulletSpawnPoint.position;
    //        bullet.transform.rotation = bulletSpawnPoint.rotation;
    //        bullet.SetupBullet(weaponSO.bulletSpeed, weaponSO.damage);
    //        bullet.gameObject.SetActive(true);
    //    }
    //}
}
