using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperGaming.ZombieShooter.Zombie
{
    /// <summary>
    /// this is Big Zombie class
    /// all the functionalities are mentioned in the base class
    /// we can override here
    /// </summary>
    public class BigZombie : Zombie
    {
        protected override void Start()
        {
            base.Start();
        }

        protected override void ExecuteAttack()
        {
            base.ExecuteAttack();
        }

        protected override void Die()
        {
            base.Die();
        }
    }
}
