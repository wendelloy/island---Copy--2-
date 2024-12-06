using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Added for UI handling

public class BoatControllers : MonoBehaviour
{
    public float speed = 10f;
    public float turnSpeed = 5f;
    public float speedIncrement = 2f;
    public float turnSpeedIncrement = 0.5f;
    public float increaseInterval = 15f;
    public float maxSpeed = 50f;
    public float maxTurnSpeed = 20f;

    public int lives = 3;

    private float nextSpeedIncreaseTime;
    private bool isInIslandCollision = false;
    private IslandSpawner islandSpawner;
    private bool isGameOver = false;

    public GameObject finishLinePrefab;
    public int targetIslandCount = 3;

    private int islandCount = 0;

    //public TMP_Text livesText;
    public TMP_Text gameOverText;

    public GameObject islandInfoCanvas;
    public TMP_Text islandInfoText;

    // New: Heart images for lives
    public Image[] heartImages;

    void Start()
    {
        nextSpeedIncreaseTime = Time.time + increaseInterval;
        islandSpawner = FindObjectOfType<IslandSpawner>();

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }

        if (islandInfoCanvas != null)
        {
            islandInfoCanvas.SetActive(false);
        }

        UpdateLivesText();
    }

    void Update()
    {
        if (isGameOver) return;

        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        float horizontal = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontal * turnSpeed * Time.deltaTime);

        if (Time.time >= nextSpeedIncreaseTime)
        {
            IncreaseSpeedAndTurnSpeed();
            nextSpeedIncreaseTime = Time.time + increaseInterval;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Island"))
        {
            Debug.Log("Boat hit the island!");
            islandCount++;
            Debug.Log($"Island {islandCount} reached!");
            Destroy(collision.gameObject);

            ShowIslandInfo("Island Name: Ancient Island\nDescription: A mysterious island filled with treasures!");
            StartCoroutine(ShowIslandInfoForSeconds(7f));

            if (islandCount == targetIslandCount)
            {
                islandSpawner.SpawnIslandOrFinishLine();
            }

            islandSpawner.PauseIslandSpawn();
            isInIslandCollision = true;

            StartCoroutine(PauseGameForSeconds(7f));
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Boat collided with an obstacle!");

            lives--;
            UpdateLivesUI();
            Destroy(collision.gameObject);

            if (lives <= 0)
            {
                Debug.Log("Game Over!");
                EndGame();
            }
            else
            {
                Debug.Log($"Lives remaining: {lives}");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishLine"))
        {
            Debug.Log("Congratulations! You reached the finish line!");
            EndGame();
        }
    }

    void IncreaseSpeedAndTurnSpeed()
    {
        if (speed < maxSpeed)
        {
            speed += speedIncrement;
        }

        if (turnSpeed < maxTurnSpeed)
        {
            turnSpeed += turnSpeedIncrement;
        }

        Debug.Log($"Speed increased to {speed}, Turn speed increased to {turnSpeed}");
    }

    IEnumerator PauseGameForSeconds(float seconds)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(seconds);
        Time.timeScale = 1;

        if (isInIslandCollision)
        {
            islandSpawner.ResumeIslandSpawn();
            isInIslandCollision = false;
        }
    }

    void EndGame()
    {
        isGameOver = true;
        Time.timeScale = 0;

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);

            if (lives == 3)
            {
                gameOverText.text = "Gold Medal! You finished with all lives!";
            }
            else if (lives == 2)
            {
                gameOverText.text = "Silver Medal! Great effort!";
            }
            else if (lives == 1)
            {
                gameOverText.text = "Bronze Medal! Nice try!";
            }
            else
            {
                gameOverText.text = "Game Over! Better luck next time!";
            }

            StartCoroutine(RestartGameAfterDelay(2f));
        }
    }

    IEnumerator RestartGameAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        lives = 3;
        islandCount = 0;

        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateLivesText()
    {
        //if (livesText != null)
        {
        //    livesText.text = "Lives: " + lives;
        }
    }

    void UpdateLivesUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].enabled = i < lives;
        }
    }

    void ShowIslandInfo(string infoText)
    {
        if (islandInfoCanvas != null && islandInfoText != null)
        {
            islandInfoCanvas.SetActive(true);
            islandInfoText.text = infoText;
        }
    }

    IEnumerator ShowIslandInfoForSeconds(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        islandInfoCanvas.SetActive(false);
    }
}
