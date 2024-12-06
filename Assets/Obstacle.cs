using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour
{
    private boat boat;

    private void Start()
    {
        boat = GameObject.FindObjectOfType<boat>();
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "boat")
        {
            Debug.Log("Boat collided with an obstacle.");
            boat.Die();
        }
    }*/
}
