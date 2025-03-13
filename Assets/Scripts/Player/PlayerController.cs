using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{
    [SerializeField] private Transform weaponSpawnPoint; // Where the weapon will appear
    [SerializeField] private float health = 100;
    private Weapon currentWeapon; // Store the currently selected weapon

    private void Start()
    {
        // Instantiate the selected weapon when the game starts
        InstantiateSelectedWeapon();
    }

    private void InstantiateSelectedWeapon()
    {
        // Get the currently selected weapon from GameplayController
        if (GameplayController.Instance != null)
        {
            GameObject weaponPrefab = GameplayController.Instance.GetCurrentWeaponPrefab();

            if (weaponPrefab != null)
            {
                // Instantiate the weapon at the spawn point
                GameObject weaponObject = Instantiate(weaponPrefab, weaponSpawnPoint.position, weaponSpawnPoint.rotation, weaponSpawnPoint);
                currentWeapon = weaponObject.GetComponent<Weapon>(); // Get the Weapon component from the instantiated object

                if (currentWeapon != null)
                {
                    Debug.Log("Equipped Weapon: " + currentWeapon.name);
                }
                else
                {
                    Debug.LogWarning("Weapon component not found on the instantiated weapon prefab.");
                }
            }
            else
            {
                Debug.LogWarning("No weapon prefab found in GameplayController!");
            }
        }
        else
        {
            Debug.LogWarning("GameplayController is null");

        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        gameObject.SetActive(false);
        Debug.Log("GAME OVER");
    }

    //public virtual void Die()
    //{
    //    isDead = true;
    //    isAttacking = false;
    //    PlayDeathAnimation();
    //    StartCoroutine(DeactivateAfterDeath());
    //}

    // Coroutine to deactivate zombie after death animation
    private IEnumerator DeactivateAfterDeath()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
