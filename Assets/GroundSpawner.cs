using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [SerializeField] GameObject groundTile;  // Reference to the ground tile prefab
    Vector3 nextSpawnPoint;  // Position to spawn the next tile

    // Method to spawn a tile
    public void SpawnTile(bool spawnObstacles)
    {
        // Instantiate a new ground tile at the next spawn point
        GameObject temp = Instantiate(groundTile, nextSpawnPoint, Quaternion.identity);

        // Update the next spawn point to the position of the new tile's end
        nextSpawnPoint = temp.transform.GetChild(1).transform.position;

        // Spawn obstacles if needed
        if (spawnObstacles)
        {
            temp.GetComponent<GroundTile>().SpawnObstacle();
        }
    }

    private void Start()
    {
        // Initially spawn 15 tiles
        for (int i = 0; i < 15; i++)
        {
            // First 3 tiles without obstacles
            if (i < 3)
            {
                SpawnTile(false);  // No obstacles
            }
            else
            {
                SpawnTile(true);  // Spawn obstacles on the rest
            }
        }
    }
}
