using UnityEngine;

public class IslandSpawner : MonoBehaviour
{
    public GameObject islandPrefab;      // Prefab for the island
    public GameObject finishLinePrefab;  // Prefab for the finish line
    public float spawnInterval = 15f;    // Time interval for spawning islands
    public float spawnDistance = 50f;    // Distance ahead of the boat to spawn the island or finish line
    public Vector2 spawnRangeX = new Vector2(-5f, 5f); // Horizontal range for spawning objects
    public int finishLineSpawnCount = 3; // Number of islands before spawning the finish line

    private Transform boatTransform;     // Reference to the boat
    private bool isFinishLineSpawned = false; // Flag to track if the finish line is spawned
    private int islandSpawnCount = 0;    // Counter for the number of islands spawned

    void Start()
    {
        // Find the boat in the scene
        GameObject boat = GameObject.Find("Boat");
        if (boat == null)
        {
            Debug.LogError("Boat not found!");
            return;
        }

        boatTransform = boat.transform;

        // Start spawning islands
        InvokeRepeating("SpawnIslandOrFinishLine", spawnInterval, spawnInterval);
    }

    // Change method access modifier to public
    public void SpawnIslandOrFinishLine()
    {
        if (boatTransform == null) return;

        // Calculate spawn position
        Vector3 spawnPosition = new Vector3(
            Random.Range(spawnRangeX.x, spawnRangeX.y), // Random X position
            boatTransform.position.y + 1f,             // Y position aligned with the boat
            boatTransform.position.z + spawnDistance   // Distance ahead of the boat
        );

        // Spawn the finish line if the required number of islands has been spawned
        if (!isFinishLineSpawned && islandSpawnCount >= finishLineSpawnCount)
        {
            Instantiate(finishLinePrefab, spawnPosition, Quaternion.identity);
            isFinishLineSpawned = true;
            Debug.Log("Finish line spawned!");
        }
        else
        {
            // Otherwise, spawn a regular island
            Instantiate(islandPrefab, spawnPosition, Quaternion.identity);
            islandSpawnCount++;
            Debug.Log($"Island {islandSpawnCount} spawned!");
        }
    }

    public void PauseIslandSpawn()
    {
        CancelInvoke("SpawnIslandOrFinishLine"); // Pause spawning
    }

    public void ResumeIslandSpawn()
    {
        InvokeRepeating("SpawnIslandOrFinishLine", spawnInterval, spawnInterval); // Resume spawning
    }
}
