using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperGaming.ZombieShooter.Weapons
{
    /// <summary>
    /// derived pistol class from the base weapon class
    /// </summary>
    public class Pistol : Weapon
    {
        protected override void Start()
        {
            base.Start();  // Initialize base class variables
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && CanFire())
            {
                Fire();
            }
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

        //private void ShootBullet()
        //{
        //    Bullet bullet = objectPool.GetPooledObject();
        //    if (bullet != null)
        //    {
        //        bullet.transform.position = bulletSpawnPoint.position;
        //        //bullet.transform.rotation = bulletSpawnPoint.rotation;
        //        bullet.SetupBullet(weaponSO.bulletSpeed, weaponSO.damage);
        //        bullet.gameObject.SetActive(true);
        //    }
        //}
    }
}
