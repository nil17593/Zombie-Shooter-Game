using UnityEngine;
namespace SuperGaming.ZombieShooter.Zombie
{
    public class ZombieAttackingState : IZombieState
    {
        private Zombie zombie;
        private float attackCooldownTimer;

        public void Enter(Zombie zombie)
        {
            this.zombie = zombie;
            //attackCooldownTimer = zombie.attackCooldown;  // Initialize the cooldown timer
            // zombie.PlayAttackAnimation();  // Trigger attack animation
        }

        public void UpdateState()
        {
            // Handle attack cooldown
            if (attackCooldownTimer > 0)
            {
                attackCooldownTimer -= Time.deltaTime;
            }
            else
            {
                // Attack the player
                //zombie.ExecuteAttack();
                //attackCooldownTimer = zombie.attackCooldown;  // Reset the attack cooldown
            }

            // If zombie is no longer near the player, switch back to moving state
            //if (!zombie.IsNearPlayer())
            {
                //zombie.SetState(new ZombieMovingState());
            }
        }

        public void Exit()
        {
            // Cleanup actions if necessary
        }
    }
}