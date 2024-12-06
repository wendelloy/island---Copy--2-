using UnityEngine;
using System.Collections.Generic;

public class WaterSpawner : MonoBehaviour
{
    public GameObject waterTilePrefab; // Prefab for the water tile
    public Transform boatTransform;   // Reference to the boat
    public int numberOfTiles = 5;     // Number of water tiles to maintain ahead of the boat
    public float tileLength = 10f;    // Length of each water tile

    private Queue<GameObject> activeTiles = new Queue<GameObject>(); // Queue to manage active water tiles
    private float spawnZ;             // Z position for the next water tile
    private float boatStartZ;         // Boat's initial Z position

    void Start()
    {
        if (boatTransform == null)
        {
            Debug.LogError("Boat transform is not assigned!");
            return;
        }

        boatStartZ = boatTransform.position.z;
        spawnZ = boatStartZ;

        // Spawn initial tiles
        for (int i = 0; i < numberOfTiles; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        // Check if the boat has moved far enough to spawn a new tile
        if (boatTransform.position.z >= spawnZ - (numberOfTiles * tileLength))
        {
            SpawnTile();
            RemoveTile();
        }
    }

    void SpawnTile()
    {
        // Spawn a new tile at the current spawn Z position
        Vector3 spawnPosition = new Vector3(0, 0, spawnZ);
        GameObject tile = Instantiate(waterTilePrefab, spawnPosition, Quaternion.identity);
        activeTiles.Enqueue(tile);

        // Move the spawn position forward
        spawnZ += tileLength;
    }

    void RemoveTile()
    {
        // Remove the oldest tile if we have more tiles than needed
        if (activeTiles.Count > numberOfTiles)
        {
            GameObject oldTile = activeTiles.Dequeue();
            Destroy(oldTile);
        }
    }
}
