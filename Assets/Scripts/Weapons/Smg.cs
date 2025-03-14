using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperGaming.ZombieShooter.Weapons
{
    /// <summary>
    /// SMG class derived from base weapon class
    /// </summary>
    public class Smg : Weapon
    {
        protected override void Start()
        {
            base.Start();  // Initialize base class variables
        }
        private void Update()
        {
            if (Input.GetMouseButton(0))  // Hold down the fire button for automatic fire
            {
                if (CanFire())
                {
                    Fire();
                }
            }
        }
        public override void Fire()
        {
            if (!CanFire()) return;

            lastShotTime = Time.time;  // Update shot time

            ShootBullet();

            if (currentAmmo <= 0)
            {
                StartCoroutine(Reload());
            }
        }
    }
}