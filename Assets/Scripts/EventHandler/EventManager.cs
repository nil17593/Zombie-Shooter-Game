using System;
using System.Collections.Generic;

namespace SuperGaming.ZombieShooter.Event
{
    /// <summary>
    /// This is the event Manager class gets different created events and triggers them when needed
    /// </summary>
    public static class EventManager
    {
        // Define events for player death and all zombies killed
        public static event Action OnPlayerKilled;
        public static event Action OnAllZombiesKilled;
        public static event Action<int> OnAllZombieWavesSpawned;
        public static event Action OnZombieKilled;

        // Trigger the PlayerKilled event
        public static void TriggerPlayerKilledEvent()
        {
            OnPlayerKilled?.Invoke();
        }

        // Trigger the AllZombiesKilled event
        public static void TriggerAllZombiesKilledEvent()
        {
            OnAllZombiesKilled?.Invoke();
        }

        public static void TriggerAllZombiesSpawnedEvent(int count)
        {
            OnAllZombieWavesSpawned?.Invoke(count);
        }
        public static void TriggerZombieKilledEvent()
        {
            OnZombieKilled?.Invoke();
        }
    }
}


