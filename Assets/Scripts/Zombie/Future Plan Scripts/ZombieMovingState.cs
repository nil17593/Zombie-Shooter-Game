namespace SuperGaming.ZombieShooter.Zombie
{
    public class ZombieMovingState : IZombieState
    {
        private Zombie zombie;

        public void Enter(Zombie zombie)
        {
            this.zombie = zombie;
        }

        public void UpdateState()
        {
            // Move the zombie toward the player
            //zombie.Move();

            // If zombie is near the player, switch to attacking state
            //if (zombie.IsNearPlayer())
            {
                //zombie.SetState(new ZombieAttackingState());
            }
        }

        public void Exit()
        {
            // Cleanup actions if necessary
        }
    }
}