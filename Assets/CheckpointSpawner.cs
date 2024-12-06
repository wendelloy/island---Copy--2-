using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckpointSpawner : MonoBehaviour
{
    public GameObject checkpointPrefab;       // Reference to the checkpoint prefab
    public GameObject lastIslandPrefab;       // Reference to the last island prefab (final checkpoint)
    public float spawnInterval = 5f;          // Time in seconds between checkpoint spawns
    public float spawnDistance = 1000f;       // Distance in front of the boat to spawn checkpoint
    public float delayAfterCheckpoint = 7f;   // 7-second delay after reaching checkpoint

    private Transform boatTransform;          // Reference to the boat's transform
    private float mainTimer = 0f;             // Main timer for countdown
    private bool delayActive = false;         // Whether the delay timer is active
    private bool waitingForCheckpoint = false; // Whether waiting for the player to reach checkpoint

    private int totalSpawnedCheckpoints = 0;   // Tracks how many checkpoints have been spawned
    public int maxCheckpoints = 10;            // Set the max checkpoints in the Inspector
    private int score = 0;                     // Score based on the checkpoints passed

    private bool lastIslandSpawned = false;   // Flag to check if the last island is spawned
    private bool gameEnded = false;           // Flag to track if the game has ended

    void Start()
    {
        boatTransform = GameObject.FindWithTag("Player").transform;
        Debug.Log("Initial main timer: " + spawnInterval + " seconds");
    }

    void Update()
    {
        // Main timer counts up only if not waiting for checkpoint or delay
        if (!delayActive && !waitingForCheckpoint)
        {
            mainTimer += Time.deltaTime;

            // Check if main timer has reached spawn interval
            if (mainTimer >= spawnInterval)
            {
                if (totalSpawnedCheckpoints < maxCheckpoints)
                {
                    SpawnCheckpoint();
                }
                else if (!lastIslandSpawned)
                {
                    // Don't spawn the last island yet, wait for 5 seconds after the last checkpoint
                    lastIslandSpawned = true; // Set flag to indicate we are ready to spawn the last island
                    StartCoroutine(SpawnLastIslandDelayed(5f));  // Call delayed spawn for last island
                }
                mainTimer = 0f; // Reset main timer after spawning a checkpoint
                waitingForCheckpoint = true; // Now wait for player to reach checkpoint
            }
        }
    }

    void SpawnCheckpoint()
    {
        Vector3 spawnPosition = boatTransform.position + boatTransform.forward * spawnDistance;

        // Instantiate the checkpoint
        GameObject checkpoint = Instantiate(checkpointPrefab, spawnPosition, Quaternion.identity);

        // Get the checkpoint's Checkpoint script and set the spawn limit
        Checkpoint checkpointScript = checkpoint.GetComponent<Checkpoint>();
        if (checkpointScript != null)
        {
            checkpointScript.spawnLimit = 1;  // Set the spawn limit for each checkpoint
        }

        totalSpawnedCheckpoints++;  // Increment the count for spawned checkpoints
        score++;  // Increase the score for each checkpoint passed
        Debug.Log("Checkpoint spawned at position: " + spawnPosition);
    }

    // Method to spawn the last island after a delay
    IEnumerator SpawnLastIslandDelayed(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);  // Wait for the specified delay (5 seconds)
        SpawnLastIsland();  // Spawn the last island
    }

    // Method to spawn the last island (end game island)
    void SpawnLastIsland()
    {
        Vector3 spawnPosition = boatTransform.position + boatTransform.forward * spawnDistance;

        // Instantiate the last island (final checkpoint)
        Instantiate(lastIslandPrefab, spawnPosition, Quaternion.identity);

        // Log the score and end the game after spawning the last island
        EndGame();
    }

    public void PlayerReachedCheckpoint()
    {
        StartCoroutine(CheckpointDelay());
    }

    private IEnumerator CheckpointDelay()
    {
        Debug.Log("Player reached checkpoint! Starting 7-second delay.");
        delayActive = true;

        // Wait for the specified delay
        yield return new WaitForSecondsRealtime(delayAfterCheckpoint);

        delayActive = false;
        waitingForCheckpoint = false;

        Debug.Log("Delay ended, main timer resumed.");
        mainTimer = 0f;  // Reset main timer after delay
    }

    // Method to end the game and show the score
    private void EndGame()
    {
        if (!gameEnded)
        {
            Debug.Log("Game Over! You reached the last island.");
            Debug.Log("Your Score: " + score);
            Time.timeScale = 0f;  // Stop the game
            gameEnded = true;  // Ensure the game only ends once
        }
    }
}
