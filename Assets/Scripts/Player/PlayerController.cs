using System.Collections;
using SuperGaming.ZombieShooter.Controllers;
using SuperGaming.ZombieShooter.Events;
using SuperGaming.ZombieShooter.UI;
using SuperGaming.ZombieShooter.Weapons;
using UnityEngine;

namespace SuperGaming.ZombieShooter.Player
{
    public class PlayerController : MonoBehaviour, IDamagable
    {
        [Header("Player Settings")]
        [SerializeField] private Transform weaponSpawnPoint; // Where the weapon will appear
        [SerializeField] private float health = 100;
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private Animator animator;

        #region Cached data
        private Weapon currentWeapon; // Store the currently selected weapon
        private float maxhealth;
        #endregion

        private void Start()
        {
            maxhealth = health;
            // Instantiate the selected weapon when the game starts
            InstantiateSelectedWeapon();
            healthBar.SetHealth(health, maxhealth);
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
            healthBar.SetHealth(health, maxhealth);
            if (health <= 0)
            {
                if (GameplayController.Instance != null)
                {
                    GameplayController.Instance.IsGameOver = true;
                }
                Die();
            }
        }

        void Die()
        {
            animator.Play("Die");
            EventManager.TriggerPlayerKilledEvent();
        }
    }
}