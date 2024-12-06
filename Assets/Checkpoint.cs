using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int spawnLimit = 1;  // Set this in the Inspector to control how many times the island can spawn
    private int spawnCount = 0; // Tracks how many times the island has spawned

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object colliding with the island is the boat
        if (collision.gameObject.CompareTag("Player"))  // Boat must be tagged "Player"
        {
            Debug.Log("Boat has collided with the island!");

            // This could be called from CheckpointSpawner when the player reaches a checkpoint
            CheckpointSpawner checkpointSpawner = FindObjectOfType<CheckpointSpawner>();
            if (checkpointSpawner != null)
            {
                checkpointSpawner.PlayerReachedCheckpoint();  // Trigger the checkpoint action
            }

            Destroy(gameObject);  // Destroy the island after collision

            // Increment spawn count and check if we hit the spawn limit
            spawnCount++;
            if (spawnCount >= spawnLimit)
            {
                Debug.Log("Spawn limit reached for this island.");
                gameObject.SetActive(false);  // Disable the island if it exceeds the spawn limit
            }
        }
    }
}
