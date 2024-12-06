using UnityEngine;

public class GroundTile : MonoBehaviour
{
    GroundSpawner groundSpawner;  // Reference to the GroundSpawner
    [SerializeField] GameObject obstaclePrefab;  // Reference to the obstacle prefab

    private void Start()
    {
        // Find the GroundSpawner in the scene
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
    }

    private void OnTriggerExit(Collider other)
    {
        // When the ground tile exits the player's view, spawn a new tile and destroy the old one
        groundSpawner.SpawnTile(true);  // Create a new tile with obstacles
        Destroy(gameObject, 5);  // Destroy this tile after 2 seconds to prevent clutter
    }

    // Method to spawn obstacles on the ground tile
    public void SpawnObstacle()
    {
        // Randomly pick a location to spawn an obstacle (pick a random child index for variety)
        int obstacleSpawnIndex = Random.Range(2, 5);  // Randomly pick from child indices 2 to 5
        Transform spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;

        // Instantiate the obstacle at the selected position
        Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity, transform);
    }
}
