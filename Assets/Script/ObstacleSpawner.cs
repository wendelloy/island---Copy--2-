using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    public List<GameObject> obstaclePrefabs; // List of obstacle prefabs to spawn
    public float spawnRangeX = 5f;           // Horizontal range for obstacle positions
    public float spawnDistanceZ = 10f;       // Distance between consecutive rows of obstacles
    public int maxObstacles = 10;           // Number of rows of obstacles to maintain ahead of the boat
    public int obstaclesPerRow = 3;         // Number of obstacles per row
    public float movingObstacleChance = 0.2f; // Chance for obstacles to move

    private List<GameObject> activeObstacles = new List<GameObject>(); // List to track active obstacles
    private Transform boatTransform;         // Reference to the boat
    private float nextSpawnZ;                // Z position for the next row

    void Start()
    {
        // Ensure the list of prefabs is not empty
        if (obstaclePrefabs == null || obstaclePrefabs.Count == 0)
        {
            Debug.LogError("No obstacle prefabs assigned!");
            return;
        }

        // Get a reference to the boat
        GameObject boat = GameObject.Find("Boat");
        if (boat == null)
        {
            Debug.LogError("Boat not found in the scene!");
            return;
        }
        boatTransform = boat.transform;

        // Initialize the spawn position
        nextSpawnZ = boatTransform.position.z + spawnDistanceZ;

        // Spawn initial rows of obstacles
        for (int i = 0; i < maxObstacles; i++)
        {
            SpawnObstacleRow();
        }
    }

    void Update()
    {
        if (boatTransform == null) return;

        // Remove obstacles that are null or behind the boat
        for (int i = activeObstacles.Count - 1; i >= 0; i--)
        {
            GameObject obstacle = activeObstacles[i];
            if (obstacle == null || obstacle.transform.position.z < boatTransform.position.z - spawnDistanceZ)
            {
                if (obstacle != null) Destroy(obstacle);
                activeObstacles.RemoveAt(i);
            }
        }

        // Spawn new rows of obstacles to maintain count
        while (activeObstacles.Count / obstaclesPerRow < maxObstacles)
        {
            SpawnObstacleRow();
        }
    }

    void SpawnObstacleRow()
    {
        // Spawn multiple obstacles for the row
        for (int i = 0; i < obstaclesPerRow; i++)
        {
            // Calculate spawn position
            Vector3 spawnPosition = new Vector3(
                Random.Range(-spawnRangeX, spawnRangeX), // Random X position
                1,                                      // Fixed Y position
                nextSpawnZ                              // Z position for the row
            );

            // Select a random obstacle prefab from the list
            GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];

            // Instantiate the obstacle
            GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);

            // Randomly decide if this obstacle will move
            if (Random.value < movingObstacleChance)
            {
                if (obstacle.GetComponent<ObstacleHorizontalMover>() == null)
                {
                    obstacle.AddComponent<ObstacleHorizontalMover>();
                }
            }

            // Add the obstacle to the list
            activeObstacles.Add(obstacle);
        }

        // Update the next Z position for spawning
        nextSpawnZ += spawnDistanceZ;
    }
}
