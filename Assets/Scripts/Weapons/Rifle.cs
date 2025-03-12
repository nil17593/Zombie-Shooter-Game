using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    protected override void Start()
    {
        base.Start();  // Initialize base class variables
        Debug.Log("THIS IS = " + this.name);
        Debug.Log("THIS IS = " + firerate);

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

    private void ShootBullet()
    {
        // Get bullet from object pool
        //GameObject bullet = ObjectPool.Instance.GetPooledObject("Bullet");
        //if (bullet != null)
        //{
        //    bullet.transform.position = bulletSpawnPoint.position;
        //    bullet.transform.rotation = bulletSpawnPoint.rotation;
        //    bullet.SetActive(true);
        //    bullet.GetComponent<Bullet>().SetSpeed(weaponSO.bulletSpeed);
        //}
    }
}
