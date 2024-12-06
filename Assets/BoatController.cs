using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class BoatController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f; // Forward speed
    public float sideSpeed = 5f; // Left/right speed
    public float rotationSpeed = 720f; // Rotation speed in degrees per second

    private Rigidbody rb;

    [Header("Game Settings")]
    public int lives = 10; // Start with 10 lives (score)
    private bool alive = true;
    private float collisionCooldown = 1f;
    private float lastCollisionTime = -1f;

    public float speedIncreaseInterval = 3f; // Interval for speed increase in seconds
    private float lastSpeedIncreaseTime = 0f; // Time of the last speed increase
    public float speedIncreaseAmount = 250f;  // Amount to increase forward speed

    [Header("UI Settings")]
    public GameObject popUp; // Reference to the pop-up UI element
    public TextMeshProUGUI popUpText;  // Reference to the TextMeshProUGUI component of the pop-up UI

    private bool isPaused = false; // Flag for game pause
    private float pauseTimer = 0f; // Timer for the 7-second pause

    // List of messages to display in order
    private List<string> infoMessages = new List<string>
    {
        "1",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
        "8",
        "9",
        "10",
    };

    private int currentInfoIndex = 0; // Tracks which message to show

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Ensure the pop-up is hidden initially
        if (popUp != null)
        {
            popUp.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (!alive || isPaused) return;

        // Movement logic retained from the original BoatController
        Vector3 forwardMovement = transform.forward * moveSpeed * Time.deltaTime;

        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 sideMovement = transform.right * horizontalInput * sideSpeed * Time.deltaTime;

        Vector3 movement = forwardMovement + sideMovement;
        rb.MovePosition(rb.position + movement);
    }

    void Update()
    {
        if (!alive) return;

        // Increase speed every interval
        if (Time.time > lastSpeedIncreaseTime + speedIncreaseInterval)
        {
            moveSpeed += speedIncreaseAmount * Time.deltaTime;
            lastSpeedIncreaseTime = Time.time;
        }

        // Handle the 7-second pause timer
        if (isPaused)
        {
            pauseTimer += Time.unscaledDeltaTime;
            if (pauseTimer >= 7f)
            {
                ResumeGame();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && Time.time > lastCollisionTime + collisionCooldown)
        {
            lastCollisionTime = Time.time;
            HandleCollision(collision);
        }

        if (collision.gameObject.CompareTag("Island"))
        {
            Destroy(collision.gameObject); // Destroy the island checkpoint
            PauseGameForDisplay();
        }
    }

    private void HandleCollision(Collision collision)
    {
        lives--; // Deduct 1 life
        if (lives > 0)
        {
            Destroy(collision.gameObject);
        }
        else
        {
            alive = false;
            EndGame();
        }
    }

    private void EndGame()
    {
        Time.timeScale = 0; // Stop the game
        DisplayResult();
    }

    private void PauseGameForDisplay()
    {
        if (currentInfoIndex >= infoMessages.Count) return;

        isPaused = true;
        pauseTimer = 0f;
        Time.timeScale = 0; // Pause game time

        if (popUp != null && popUpText != null)
        {
            popUp.SetActive(true);
            popUpText.text = infoMessages[currentInfoIndex];
            currentInfoIndex++;
        }
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1; // Resume game time

        if (popUp != null)
        {
            popUp.SetActive(false);
        }
    }

    private void DisplayResult()
    {
        if (lives == 10)
        {
            Debug.Log("Passed (Gold)");
        }
        else if (lives == 9)
        {
            Debug.Log("Passed (Silver)");
        }
        else if (lives == 8)
        {
            Debug.Log("Passed (Bronze)");
        }
        else
        {
            Debug.Log("Retry again (Failed)");
        }
    }
}
