using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boat : MonoBehaviour
{
    public float turnSpeed = 1000f;
    public float accelerateSpeed = 1000f;
    public int boatLife = 3; // Boat life points

    private Rigidbody rbody;

    void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        rbody.AddTorque(0f, h * turnSpeed * Time.deltaTime, 0f);
        rbody.AddForce(transform.forward * v * accelerateSpeed * Time.deltaTime);
    }

    // Detect collision with a rock
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rock")) // Ensure rocks are tagged as "Rock"
        {
            boatLife--; // Decrease life by 1 on collision with rock
            Debug.Log("Boat hit a rock! Remaining life: " + boatLife);

            if (boatLife <= 0)
            {
                // Handle boat destruction or game over logic here
                Debug.Log("Boat destroyed!");
                this.enabled = false; // Disable the boat's control script
            }
        }
    }
}
