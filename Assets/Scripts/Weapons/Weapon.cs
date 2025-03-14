using SuperGaming.ZombieShooter.Pools;
using System.Collections;
using UnityEngine;
using SuperGaming.ZombieShooter.Controllers;

namespace SuperGaming.ZombieShooter.Weapons
{
    /// <summary>
    /// this is base class for the weapons
    /// manages weapon data by using scriptable object data
    /// </summary>
    public abstract class Weapon : MonoBehaviour
    {
        #region Public fields
        [Header("Basic settings for weapon")]
        public WeaponScriptableObject weaponSO;  // Scriptable object with weapon stats
        public Transform bulletSpawnPoint;
        public GameObject bulletPrefab;
        public int poolSize;
        #endregion

        #region Protected fields
        protected bool isReloading = false;      // Reload state
        protected float lastShotTime;            // Tracks time of the last shot
        protected float firerate;
        protected float bulletSpeed;
        protected float reloadDuration;
        protected int currentAmmo;
        protected ObjectPool<Bullet> objectPool;
        #endregion

        protected virtual void Start()
        {
            Initialize(weaponSO);
            currentAmmo = weaponSO.magSize;

            objectPool = new ObjectPool<Bullet>(bulletPrefab, this.transform, poolSize);
            UIController.Instance.SetCurrentBulletText(currentAmmo, weaponSO.magSize);
        }

        void Initialize(WeaponScriptableObject weaponScriptableObject)
        {
            // Set weapon properties from scriptable object
            firerate = 1 / (weaponScriptableObject.fireRate / 60f);
            bulletSpeed = weaponScriptableObject.bulletSpeed;
            reloadDuration = weaponScriptableObject.reloadDuration;
            currentAmmo = weaponScriptableObject.magSize;
        }

        // Abstract method for firing (specific firing logic will be in subclasses)
        public abstract void Fire();

        // Reload coroutine (common behavior across all weapons)
        protected IEnumerator Reload()
        {
            isReloading = true;
            UIController.Instance.SetCurrentBulletText(currentAmmo, weaponSO.magSize, isReloading);
            yield return new WaitForSeconds(weaponSO.reloadDuration);
            currentAmmo = weaponSO.magSize;
            isReloading = false;
            UIController.Instance.SetCurrentBulletText(currentAmmo, weaponSO.magSize, isReloading);
        }

        // Utility method to check if the weapon can fire (common to all weapons)
        protected bool CanFire()
        {
            return !isReloading && currentAmmo > 0 && (Time.time - lastShotTime >= firerate);
        }


        protected virtual void ShootBullet()
        {
            Bullet bullet = objectPool.GetPooledObject();
            if (bullet != null)
            {
                currentAmmo--;
                bullet.transform.position = bulletSpawnPoint.position;
                //bullet.transform.rotation = bulletSpawnPoint.rotation;
                bullet.SetupBullet(weaponSO.bulletSpeed, weaponSO.damage);
                bullet.gameObject.SetActive(true);
                UIController.Instance.SetCurrentBulletText(currentAmmo, weaponSO.magSize);
            }
        }
    }
}
