using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform player;
    private float minSpawnDistance = 20f;
    private float maxSpawnDistance = 40f;
    public float spawnInterval = 2f;

    public PlayerController playerController;
    public int zombiesOnPlayer = 0;
    public int zombiesRunning = 0;
    public int totalZombiesInWave = 0; // Number of zombies in the current wave
    public int zombiesToSpawn = 0; // Number of zombies left to spawn in the wave

    public SoundManager soundManager;

    public int currentWave = 1;
    private int totalWaves = 3;
    private bool waveInProgress = false;

    public Slider waveProgressBar;
    public int zombiesInWave; //used to update the slider value

    public UpgradeManager upgradeManager;
    public int activeZombies = 0; // Number of zombies currently alive


    void Start()
    {
        //InvokeRepeating(nameof(SpawnZombie), startWaveOne, spawnInterval);
        StartCoroutine(HandleWaves());

    }

    private void Update()
    {
        if (zombiesOnPlayer > 0 && Time.time >= soundManager.nextAttackSound)
        {
            soundManager.ZombieAttack();
        }

        if (zombiesRunning >= 3 && Time.time >= soundManager.nextRunningSound)
        {
            soundManager.ZombieRunning();
        }

        WaveProgressSlider();
        
    }

    private void WaveProgressSlider()
    {
        waveProgressBar.value = zombiesInWave;
    }

    private void SpawnZombie()
    {
        // Calculate a random spawn distance
        float spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);

        // Calculate a random position around the player
        Vector3 randomDirection = Random.insideUnitSphere * spawnDistance;
        randomDirection.y = 0; // Keep the spawn point level with the player

        // Create the spawn position based on the player's position
        Vector3 spawnPosition = player.position + randomDirection;

        // Use a raycast to find the height of the terrain at the spawn position
        RaycastHit hit;
        if (Physics.Raycast(spawnPosition + Vector3.up * 10f, Vector3.down, out hit, 20f)) // Adjust height as needed
        {
            spawnPosition.y = hit.point.y; // Set the spawn position Y to the terrain height
        }
        else
        {
            // If the raycast does not hit anything, you can choose to keep the original spawnPosition.y or handle the error
            spawnPosition.y = 0; // Default height (make sure this is suitable for your game)
        }

       
        
        

        // Instantiate the zombie prefab at the adjusted spawn position
        Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

        // Assign the player reference to the zombie's movement script
        ZombieBehaviour zombieBehaviour = zombiePrefab.GetComponent<ZombieBehaviour>();
        
        
        if (zombieBehaviour != null)
        {
            zombieBehaviour.player = player; // Set the player reference
            zombieBehaviour.zombieSpawner = this;
            zombieBehaviour.currentWave = currentWave;
            
        }

        activeZombies++;
    }

    public void ZombieDied()
    {
        activeZombies--; // Decrement active zombies count
    }

    private IEnumerator HandleWaves()
    {
        // Wait 10 seconds before starting wave 1
        //yield return new WaitForSeconds(10f);


        while (currentWave <= totalWaves)
        {
            waveInProgress = true;
            StartWave(currentWave);

            // Wait until all zombies in the wave are dead
            yield return new WaitUntil(() => activeZombies == 0 && zombiesToSpawn == 0);

            waveInProgress = false;

            //upgradeManager.waveBreak = true;
            //upgradeManager.StartCountdown();

            if (currentWave < totalWaves)
            {
                // Wait 30 seconds before starting the next wave
                //yield return new WaitForSeconds(30f);
                //upgradeManager.waveBreak = false;

                //upgradeManager.StartCountdown(); // Start the countdown
                yield return new WaitForSeconds(upgradeManager.waveInterval); // Wait for the countdown to finish
            }

            //currentWave++;
        }
    }

    private void StartWave(int waveNumber)
    {
        switch (waveNumber)
        {
            case 1:
                totalZombiesInWave = 20; // Number of zombies for wave 1
                break;
            case 2:
                totalZombiesInWave = 25; // Number of zombies for wave 2
                break;
            case 3:
                totalZombiesInWave = 30; // Number of zombies for wave 3
                break;
        }

        waveProgressBar.maxValue = totalZombiesInWave;
        zombiesToSpawn = totalZombiesInWave;
        zombiesInWave = totalZombiesInWave;
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        while (zombiesToSpawn > 0)
        {
            SpawnZombie();
            zombiesToSpawn--;

            // Wait for the spawn interval before spawning the next zombie
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void NextWave()
    {
        // This method is called when the countdown is finished in UpgradeManager
        // You can start spawning zombies for the next wave here if needed
        if (currentWave < totalWaves)
        {
            currentWave++;
            StartCoroutine(HandleWaves());
        }
    }

}
