using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZombieStats", menuName = "ZombieScriptableObjects/ZombieStats", order = 1)]
public class ZombieScriptableObject : ScriptableObject
{
    public int health;
    public int damage;
    public float moveSpeed;
    public float attackRange;
    public float attackCooldown;  // Cooldown between attacks
}
