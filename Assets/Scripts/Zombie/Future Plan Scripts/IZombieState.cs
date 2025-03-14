namespace SuperGaming.ZombieShooter.Zombie
{
    public interface IZombieState
    {
        void Enter(Zombie zombie);  // Called when entering the state
        void UpdateState();         // Called each frame to update the state
        void Exit();                // Called when exiting the state
    }
}
