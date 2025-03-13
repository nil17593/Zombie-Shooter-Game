using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "WeaponScriptableObject/WeaponData")]
public class WeaponScriptableObject : ScriptableObject
{
    [Header("Name")]
    public string weaponName;
    public WeaponType weaponType;
    [Header("Weapon Settings")]
    public int damage;
    public int magSize;
    public float reloadDuration;
    public float fireRate;//RPM (Rounds Per Minute)
    public float bulletSpeed;//speed in m/s
    public Sprite weaponSprite;
    public bool isAutomatic;
}
