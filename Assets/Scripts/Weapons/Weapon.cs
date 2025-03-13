using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public WeaponScriptableObject weaponSO;  // Scriptable object with weapon stats
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    protected bool isReloading = false;      // Reload state
    protected float lastShotTime;            // Tracks time of the last shot
    protected float firerate;
    protected float bulletSpeed;
    protected float reloadDuration;
    protected int currentAmmo;

    protected ObjectPool<Bullet> objectPool;
    public int poolSize;
    // For automatic weapons, we will need to track whether the player is holding down the fire button
    protected bool isFiring = false;
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

    private void Update()
    {
        // Handle automatic fire if the weapon supports it
        if (weaponSO.isAutomatic)
        {
            if (Input.GetMouseButton(0))  // Hold down the fire button for automatic fire
            {
                if (CanFire())
                {
                    Fire();
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && CanFire())
            {
                Fire();
            }
        }
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
