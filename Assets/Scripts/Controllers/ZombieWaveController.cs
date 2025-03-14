using System.Collections;
using SuperGaming.ZombieShooter.Events;
using SuperGaming.ZombieShooter.Pools;
using UnityEngine;

namespace SuperGaming.ZombieShooter.Controllers
{
    /// <summary>
    /// This class Handles the Zombie wave spawning
    /// zombies will spawn in waves
    /// using object pool zombies
    /// </summary>
    public class ZombieWaveController : MonoBehaviour
    {
        [Header("Spawning settings")]
        [SerializeField] private GameObject smallZombiePrefab;
        [SerializeField] private GameObject bigZombiePrefab;
        [Tooltip("SEt the Initial wave size")]
        [SerializeField] private int initialWaveSize = 5;
        [SerializeField] private float timeBetweenWaves = 15f;
        [SerializeField] private int maxWaves = 3; // Limit to 3 waves as mentioned in requirment
        [Tooltip("Set the difficulty by increasing the Zombie count for each wave")]
        [SerializeField] private int zombieIncreaseCountPerWave;
        [Tooltip("Can be found Under the Backroung gameobject")]
        [SerializeField] private BoxCollider2D zombieSpawnArea;

        #region Private fields
        private int currentWave = 0;
        private ObjectPool<Zombie.Zombie> smallZombiePool;
        private ObjectPool<Zombie.Zombie> bigZombiePool;
        public bool allWavesCompleted = false;
        private int zombiesAlive = 0;
        #endregion

        private void Start()
        {
            // Create object pools for both zombie types
            smallZombiePool = new ObjectPool<Zombie.Zombie>(smallZombiePrefab, this.transform, 10);
            bigZombiePool = new ObjectPool<Zombie.Zombie>(bigZombiePrefab, this.transform, 5);

            // Subscribe to the zombie death event to reduce the count of zombies alive
            EventManager.OnZombieKilled += OnZombieKilled;

            // Start the first wave
            StartCoroutine(SpawnWaves());
        }

        // Coroutine to spawn waves when all zombies from the previous wave are killed
        private IEnumerator SpawnWaves()
        {
            if (GameplayController.Instance != null && GameplayController.Instance.IsGameOver)
                yield break;

            while (!allWavesCompleted)
            {
                // Wait until all zombies from the current wave are killed
                while (zombiesAlive > 0)
                {
                    yield return null;
                }

                if (currentWave < maxWaves)
                {
                    currentWave++;
                    UIController.Instance.SetWaveText(currentWave);

                    // Increase difficulty with each wave
                    int zombiesToSpawn = initialWaveSize + currentWave * zombieIncreaseCountPerWave; // Increase zombie count with each wave
                    SpawnZombieWave(zombiesToSpawn);
                }
                else
                {
                    allWavesCompleted = true;
                    if (GameplayController.Instance != null)
                        GameplayController.Instance.IsAllWavesSpwned = allWavesCompleted;
                }

                // Wait for a bit before allowing the next wave
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }

        // Spawn a wave of zombies
        private void SpawnZombieWave(int numZombies)
        {
            zombiesAlive = numZombies;  // Set zombies alive for this wave
            Bounds zombieSpawnBounds = zombieSpawnArea.bounds;
            for (int i = 0; i < numZombies; i++)
            {
                    Vector2 randomSpawnPosition = new Vector2(
                    Random.Range(zombieSpawnBounds.min.x, zombieSpawnBounds.max.x),
                    zombieSpawnBounds.max.y);


                // Decide whether to spawn a small or big zombie
                Zombie.Zombie zombieToSpawn = (i % 2 == 0) ? smallZombiePool.GetPooledObject() : bigZombiePool.GetPooledObject();

                zombieToSpawn.transform.position = randomSpawnPosition;
                zombieToSpawn.gameObject.SetActive(true);

                zombieToSpawn.Initialize(zombieToSpawn.zombieScriptableObject);

                // Increment total zombies to kill in gameplay
                if (GameplayController.Instance != null)
                    GameplayController.Instance.TotalZombiesToKill++;
            }
        }

        // Callback for when a zombie is killed
        private void OnZombieKilled(Zombie.Zombie zombie)
        {
            zombiesAlive--;
        }

        private void OnDisable()
        {
            EventManager.OnZombieKilled -= OnZombieKilled;
        }
    }
}