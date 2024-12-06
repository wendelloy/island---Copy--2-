using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    public GameObject rockPrefab; // The rock prefab to spawn
    public Transform[] spawnPoints; // Array of spawn points for rocks
    public float spawnInterval = 3f; // Interval between each spawn

    private float nextSpawnTime = 0f;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnRock();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnRock()
    {
        // Randomly select a spawn point from the array
        Transform selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Instantiate the rock at the selected spawn point's position
        Instantiate(rockPrefab, selectedSpawnPoint.position, Quaternion.identity);
    }
}
