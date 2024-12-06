using UnityEngine;

public class ObstacleHorizontalMover : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed of horizontal movement
    public float range = 3f;    // Horizontal movement range
    private float startX;

    void Start()
    {
        startX = transform.position.x;
    }

    void Update()
    {
        // Calculate new X position
        float x = startX + Mathf.Sin(Time.time * moveSpeed) * range;
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}
