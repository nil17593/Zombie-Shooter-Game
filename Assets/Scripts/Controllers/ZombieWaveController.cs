using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieWaveController : MonoBehaviour
{
    public GameObject smallZombiePrefab;
    public GameObject bigZombiePrefab;

    public Transform[] spawnPoints; // Set multiple spawn points in the scene
    public int initialWaveSize = 5;
    public float timeBetweenWaves = 5f;  // Time to wait between waves
    public int maxWaves = 3; // Limit to 3 waves

    private int currentWave = 0;
    private ObjectPool<Zombie> smallZombiePool;
    private ObjectPool<Zombie> bigZombiePool;

    private bool allWavesCompleted = false;

    // Start is called before the first frame update
    private void Start()
    {
        // Create object pools for both zombie types
        smallZombiePool = new ObjectPool<Zombie>(smallZombiePrefab, this.transform, 10);
        bigZombiePool = new ObjectPool<Zombie>(bigZombiePrefab, this.transform, 5);

        // Start the first wave
        StartCoroutine(SpawnWaves());
    }

    // Coroutine to spawn waves with delays
    private IEnumerator SpawnWaves()
    {
        while (!allWavesCompleted)
        {
            if (currentWave < maxWaves)
            {
                currentWave++;
                int zombiesToSpawn = initialWaveSize + currentWave; 
                SpawnZombieWave(zombiesToSpawn);

                yield return new WaitForSeconds(timeBetweenWaves);
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
        }
    }
}
