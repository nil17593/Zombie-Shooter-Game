using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public WeaponScriptableObject weaponSO;  // Scriptable object with weapon stats
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    protected bool isReloading = false;      // Reload state
    protected float lastShotTime;              // Tracks time of the last shot
    protected float firerate;
    protected float bulletSpeed;
    protected float reloadDuration;
    protected int currentAmmo;

    protected ObjectPool<Bullet> objectPool;
    public int poolSize;
    protected virtual void Start()
    {
        Initialize(weaponSO);
        currentAmmo = weaponSO.magSize;

        objectPool = new ObjectPool<Bullet>(bulletPrefab, this.transform, poolSize);     
    }

    void Initialize(WeaponScriptableObject weaponScriptableObject)
    {
        // Set weapon properties from scriptable object
        firerate = 60 / weaponScriptableObject.fireRate;
        bulletSpeed = weaponScriptableObject.bulletSpeed;
        reloadDuration = weaponScriptableObject.reloadDuration;
        currentAmmo = weaponScriptableObject.magSize;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && CanFire())
        {
            Fire();
        }
    }

    // Abstract method for firing (specific firing logic will be in subclasses)
    public abstract void Fire();

    // Reload coroutine (common behavior across all weapons)
    protected IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(weaponSO.reloadDuration);
        currentAmmo = weaponSO.magSize;
        isReloading = false;
        Debug.Log("Reload Complete!");
    }

    // Utility method to check if the weapon can fire (common to all weapons)
    protected bool CanFire()
    {
        return !isReloading && currentAmmo > 0 && (Time.time - lastShotTime >= 1f / weaponSO.fireRate);
    }

    protected virtual void ShootBullet()
    {
        Bullet bullet = objectPool.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.position = bulletSpawnPoint.position;
            //bullet.transform.rotation = bulletSpawnPoint.rotation;
            bullet.SetupBullet(weaponSO.bulletSpeed, weaponSO.damage);
            bullet.gameObject.SetActive(true);
        }
    }
}
