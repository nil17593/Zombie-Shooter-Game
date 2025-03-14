using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SuperGaming.ZombieShooter.Enums;

namespace SuperGaming.ZombieShooter.Controllers
{
    /// <summary>
    /// this class is used to manage the shop 
    /// show different gun selections 
    /// and equip them
    /// </summary>
    public class ShopController : MonoBehaviour
    {
        [SerializeField] private WeaponScriptableObject[] weaponScriptableObjects;

        [Header("UI Elements")]
        [SerializeField] private Image weaponImage;
        [SerializeField] private TextMeshProUGUI weaponInfoText;
        [SerializeField] private TextMeshProUGUI weaponNameText;
        [SerializeField] private TextMeshProUGUI equipButtonText;
        [SerializeField] private Button equipWeaponButton;

        private int currentWeaponIndex = 0;

        private void Start()
        {
            // Load the saved weapon or initialize the first weapon
            var savedWeaponType = GameplayController.Instance.GetSelectedWeaponType();
            currentWeaponIndex = FindWeaponIndex(savedWeaponType);
            UpdateWeaponUI();
        }

        public void NextWeapon()
        {
            currentWeaponIndex = (currentWeaponIndex + 1) % weaponScriptableObjects.Length;
            UpdateWeaponUI();
        }

        public void PreviousWeapon()
        {
            currentWeaponIndex = (currentWeaponIndex - 1 + weaponScriptableObjects.Length) % weaponScriptableObjects.Length;
            UpdateWeaponUI();
        }

        private void UpdateWeaponUI()
        {
            var selectedWeapon = weaponScriptableObjects[currentWeaponIndex];
            weaponImage.sprite = selectedWeapon.weaponSprite;
            weaponNameText.text = selectedWeapon.weaponName;

            weaponInfoText.text = $"Damage: {selectedWeapon.damage}\n" +
                                  $"Magazine Size: {selectedWeapon.magSize}\n" +
                                  $"Reload Time: {selectedWeapon.reloadDuration}s\n" +
                                  $"Fire Rate: {60 / selectedWeapon.fireRate}\n" +
                                  $"Bullet Speed: {selectedWeapon.bulletSpeed}";

            UpdateEquipButton(selectedWeapon);
        }

        public void EquipWeapon()
        {
            var selectedWeapon = weaponScriptableObjects[currentWeaponIndex];
            GameplayController.Instance.SetSelectedWeaponType(selectedWeapon.weaponType);

            equipWeaponButton.interactable = false;
            equipButtonText.text = "Equipped";

            Debug.Log($"{selectedWeapon.weaponName} equipped!");
        }

        private void UpdateEquipButton(WeaponScriptableObject selectedWeapon)
        {
            bool isEquipped = GameplayController.Instance.GetSelectedWeaponType() == selectedWeapon.weaponType;

            equipWeaponButton.interactable = !isEquipped;
            equipButtonText.text = isEquipped ? "Equipped" : "Equip";
        }

        private int FindWeaponIndex(WeaponType weaponType)
        {
            for (int i = 0; i < weaponScriptableObjects.Length; i++)
            {
                if (weaponScriptableObjects[i].weaponType == weaponType)
                    return i;
            }
            return 0; // Default to the first weapon if not found
        }
    }
}