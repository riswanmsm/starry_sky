using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TMP_Text scoreText;
    public TMP_Text timerText;
    //public TMP_Text messageText; // Text element for messages
    public TMP_Text lifeText;
    public float levelTime = 60f; // 1 minute per level

    private int score;
    private float timer;
    private int lives;

    void Awake()
    {
        // Implement Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }

    void Start()
    {
        // Initialize or reset your score, timer, and lives here
        ResetTimer();
        ResetScore();
        ResetLives(); // Add this line
        UpdateLifeText(lives);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timerText)
        {
            UpdateTimer();
        }

        if (timer <= 0)
        {
            timer = 0;  // Clamp the timer to zero
            GameManager.instance.EndGame();  // Call the end game method
        }
    }

    public void ResetTimer()
    {
        timer = levelTime;  // Set the timer to the full level time
        UpdateTimer();
    }

    public void ResetScore()
    {
        score = 0; // Reset score to 0 or keep as needed
        UpdateScore();
    }

    public void ResetLives()
    {
        lives = 3;  // Set this to the correct starting value
        UpdateLifeText(lives);
    }

    public void AddScore(int points)
    {
        //if (gameEnded) return;

        score += points;
        UpdateScore();

        if (score >= 50)
        {
            GameManager.instance.EndLevel();
        }
    }

    public void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    public void UpdateTimer()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.Ceil(timer);
        }
    }

    public void UpdateLifeText(int lives)
    {
        if (lifeText != null)
        {
            lifeText.text = "Lives: " + lives;
        }
    }

}