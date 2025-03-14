using System.Collections;
using UnityEngine;

namespace SuperGaming.ZombieShooter.Zombie
{
    public class ZombieDyingState : IZombieState
    {
        private Zombie zombie;

        public void Enter(Zombie zombie)
        {
            this.zombie = zombie;
            //zombie.PlayDeathAnimation();
            zombie.StartCoroutine(HandleDeathSequence());  // Start death sequence coroutine
        }

        private IEnumerator HandleDeathSequence()
        {
            yield return new WaitForSeconds(2f);  // Wait for 2 seconds or the length of the death animation
            zombie.gameObject.SetActive(false);  // Deactivate zombie after death animation
        }

        public void UpdateState()
        {
            // Nothing to do during death state
        }

        public void Exit()
        {
            // Zombie is deactivated
        }
    }
}