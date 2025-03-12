using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public WeaponScriptableObject weaponSO;  // Scriptable object with weapon stats
    public Transform bulletSpawnPoint;
    protected bool isReloading = false;      // Reload state
    protected float lastShotTime;              // Tracks time of the last shot
    protected float firerate;
    protected float bulletSpeed;
    protected float reloadDuration;

    protected int currentAmmo;
    protected virtual void Start()
    {
        Initialize(weaponSO);
        currentAmmo = weaponSO.magSize;
    }

    void Initialize(WeaponScriptableObject weaponScriptableObject)
    {
        // Set weapon properties from scriptable object
        firerate = weaponScriptableObject.fireRate;
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
}
