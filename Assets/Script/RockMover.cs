using UnityEngine;

public class RockMover : MonoBehaviour
{
    public float speed = 100f; // Speed at which the rock moves toward the boat
    private Transform boat; // Reference to the boat's position

    void Start()
    {
        // Find the boat GameObject using its tag and set it as the target
        boat = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (boat != null)
        {
            // Move the rock toward the boat's position
            Vector3 direction = (boat.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }
}
