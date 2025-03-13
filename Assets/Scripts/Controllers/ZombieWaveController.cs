using System.Collections;
using SuperGaming.ZombieShooter.Event;
using UnityEngine;

public class ZombieWaveController : MonoBehaviour
{
    [SerializeField] private GameObject smallZombiePrefab;
    [SerializeField] private GameObject bigZombiePrefab;
    [SerializeField] private Transform[] spawnPoints; // Set multiple spawn points in the scene
    [SerializeField] private int initialWaveSize = 5;
    [SerializeField] private float timeBetweenWaves = 5f;  // Time to wait between waves
    [SerializeField] private int maxWaves = 3; // Limit to 3 waves
    [SerializeField] private float waveWaitTime = 60f; // Wait time before spawning the next wave if zombies are not killed

    private int currentWave = 0;
    private ObjectPool<Zombie> smallZombiePool;
    private ObjectPool<Zombie> bigZombiePool;
    private bool allWavesCompleted = false;
    private int zombiesAlive = 0;
    private bool waveActive = false;
    private int totalZombiesKilled;
    // Start is called before the first frame update
    private void Start()
    {
        // Create object pools for both zombie types
        smallZombiePool = new ObjectPool<Zombie>(smallZombiePrefab, this.transform, 10);
        bigZombiePool = new ObjectPool<Zombie>(bigZombiePrefab, this.transform, 5);

        // Start the first wave
        StartCoroutine(SpawnWaves());
    }

    // Coroutine to spawn waves with delays and checks
    private IEnumerator SpawnWaves()
    {
        while (!allWavesCompleted)
        {
            if (currentWave < maxWaves)
            {
                // Wait for the current wave to be completed
                if (!waveActive && zombiesAlive == 0)
                {
                    currentWave++;
                    waveActive = true;
                    UIController.Instance.SetWaveText(currentWave);
                    int zombiesToSpawn = initialWaveSize + currentWave * 5; // Increase zombie count with each wave
                    SpawnZombieWave(zombiesToSpawn);

                    // Wait for zombies to be killed or timeout
                    yield return new WaitUntil(() => zombiesAlive == 0);
                    yield return new WaitForSeconds(timeBetweenWaves);

                    waveActive = false;
                }
                else
                {
                    // If zombies are still alive, wait for a minute before forcing the next wave
                    yield return new WaitForSeconds(waveWaitTime);
                    if (zombiesAlive > 0)
                    {
                        Debug.Log("Forcing next wave after timeout.");
                        waveActive = false;
                        zombiesAlive = 0;
                    }
                }
            }
            else
            {
                allWavesCompleted = true;
                Debug.Log("All waves completed!");
            }
        }
    }

    // Spawn a wave of zombies
    private void SpawnZombieWave(int numZombies)
    {
        Debug.Log("Spawning wave " + currentWave);

        for (int i = 0; i < numZombies; i++)
        {
            // Randomly choose a spawn point
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Decide whether to spawn a small or big zombie
            Zombie zombieToSpawn = (i % 2 == 0) ? smallZombiePool.GetPooledObject() : bigZombiePool.GetPooledObject();

            zombieToSpawn.transform.position = spawnPoint.position;
            zombieToSpawn.gameObject.SetActive(true);

            // Initialize the zombie if needed
            zombieToSpawn.Initialize(zombieToSpawn.zombieScriptableObject);

            // Increment the count of alive zombies
            zombiesAlive++;

            // Subscribe to the zombie's death event
            EventManager.OnZombieKilled += HandleZombieDeath;
            GameplayController.Instance.TotalZombiesToKill++;
        }
    }

    // Handle zombie death
    private void HandleZombieDeath()
    {
        zombiesAlive--;
        totalZombiesKilled++;
        // Unsubscribe from the death event
        EventManager.OnZombieKilled -= HandleZombieDeath;
        if (allWavesCompleted)
        {
            GameplayController.Instance.CheckForGameWin(totalZombiesKilled);
        }
        Debug.Log("Zombie killed, zombies remaining: " + zombiesAlive);
    }


}
