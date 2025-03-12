using UnityEngine;
using System.Collections.Generic;

public class GameplayController : Singletone<GameplayController>
{
    private const string SELECTED_WEAPON_KEY = "SelectedWeapon";

    [SerializeField] private Weapon[] weapons;
    private Dictionary<WeaponType, Weapon> weaponDictionary;

    public Weapon SelectedWeapon { get; private set; }

    private void Start()
    {
        InitializeWeaponDictionary();
        LoadSelectedWeapon();
    }

    private void InitializeWeaponDictionary()
    {
        weaponDictionary = new Dictionary<WeaponType, Weapon>();

        foreach (var weapon in weapons)
        {
            weaponDictionary[weapon.weaponSO.weaponType] = weapon;
        }
    }

    private void LoadSelectedWeapon()
    {
        var savedWeaponType = GetSelectedWeaponType();

        if (!weaponDictionary.TryGetValue(savedWeaponType, out var savedWeapon))
        {
            Debug.LogWarning("Saved weapon not found, defaulting to first weapon.");
            savedWeapon = weapons[0];
        }

        SelectedWeapon = savedWeapon;
    }

    public void SetSelectedWeaponType(WeaponType weaponType)
    {
        PlayerPrefs.SetInt(SELECTED_WEAPON_KEY, (int)weaponType);
        PlayerPrefs.Save();

        if (weaponDictionary.TryGetValue(weaponType, out var selectedWeapon))
        {
            SelectedWeapon = selectedWeapon;
        }
        else
        {
            Debug.LogWarning("Selected weapon type not found in dictionary!");
        }
    }

    public WeaponType GetSelectedWeaponType()
    {
        int savedWeaponIndex = PlayerPrefs.GetInt(SELECTED_WEAPON_KEY, 0);
        return (WeaponType)savedWeaponIndex;
    }

    public GameObject GetCurrentWeaponPrefab()
    {
        return SelectedWeapon?.gameObject ?? weapons[0].gameObject;
    }
}
