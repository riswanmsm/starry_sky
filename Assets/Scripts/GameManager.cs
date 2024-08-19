using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject gameOverCanvas;
    public GameObject scoreCanvas;
    public GameObject transitionMessageCanvas;  // Reference to the transition message text

    private ScoreManager scoreManager;
    private int lives = 3;
    private bool gameEnded = false;

    public static GameManager instance;

    void Awake()
    {
        // Singleton pattern to ensure only one instance of GameManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Persist the GameOverCanvas, ScoreCanvas and TransitionMessageCanvas across scenes
        DontDestroyOnLoad(gameOverCanvas);
        DontDestroyOnLoad(scoreCanvas);
        DontDestroyOnLoad(transitionMessageCanvas);

        StartCoroutine(WaitForScoreManager());
    }

    private IEnumerator WaitForScoreManager()
    {
        while (ScoreManager.instance == null)
        {
            yield return null; // Wait for the next frame
        }

        scoreManager = ScoreManager.instance;

        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager instance not found.");
        }
    }

    void Start()
    {
        Time.timeScale = 0; // Pause the game at the start
        if (mainMenuCanvas != null) mainMenuCanvas.SetActive(true);
    }

    public void StartGame()
    {
        Debug.Log("StartGame method called");

        // Reset Time.timeScale to 1 to ensure the game runs at normal speed
        Time.timeScale = 1;

        // Reset gameEnded flag to false
        gameEnded = false;

        // Hide the main menu, game over, and transition message canvases
        if (mainMenuCanvas != null) mainMenuCanvas.SetActive(false);
        if (gameOverCanvas != null) gameOverCanvas.SetActive(false);
        if (transitionMessageCanvas != null) transitionMessageCanvas.SetActive(false);

        // Show the score canvas
        if (scoreCanvas != null) scoreCanvas.SetActive(true);

        // Reset the game status (lives, score, timer)
        ResetGameStatus();

        // Reload the current scene to reset all objects to their initial states
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Time or Life finished then call this method
    public void EndGame()
    {
        if (gameEnded) return;  // Prevents multiple executions

        gameEnded = true;

        Debug.Log("Game Over! Timer reached zero or lives depleted.");
        Time.timeScale = 0;  // Freeze the game

        // Enable the game-over canvas or show game over UI here
        ShowGameOverMenu();
    }

    public void RegisterHit()
    {
        lives--;
        scoreManager.UpdateLifeText(lives);
        Debug.Log("Total Remaining Lives: " + lives);

        if (lives <= 0)
        {
            EndGame();
        }
    }

    public void ShowTransitionMessage()
    {
        if (scoreCanvas != null)
            scoreCanvas.SetActive(false); // Hide the score UI

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false); // Ensure GameOverCanvas is hidden

        if (transitionMessageCanvas != null)
        {
            transitionMessageCanvas.SetActive(true); // Show transition message
            DisablePlayerInput();
            StartCoroutine(TransitionToNextLevel());
        }
        else
        {
            Debug.LogWarning("transitionMessageCanvas is missing.");
        }
    }

    private IEnumerator TransitionToNextLevel()
    {
        yield return new WaitForSeconds(3); // Wait for 3 seconds

        // Ensure that transitionMessageCanvas is still valid
        if (transitionMessageCanvas != null)
        {
            transitionMessageCanvas.SetActive(false);
        }
        else
        {
            Debug.LogWarning("transitionMessageCanvas was destroyed or is missing.");
        }

        // Load the next level
        LoadNextLevel();
    }

    private void EnablePlayerInput()
    {
        var playerController = FindAnyObjectByType<SpaceShooterShooting>();

        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }

    private void DisablePlayerInput()
    {
        var playerController = FindAnyObjectByType<SpaceShooterShooting>();

        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }

    public void EndLevel()
    {
        Time.timeScale = 1;  // Ensure time is running
        ShowTransitionMessage();  // Show transition message instead of directly loading the next level
    }

    public void LoadNextLevel()
    {
        //SceneManager.sceneLoaded += OnSceneLoaded;  // Add event listener
        SceneManager.LoadScene(1);
        ResetGameStatus();
        scoreCanvas.SetActive(true); // Show the score UI
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  // Remove event listener
        if (scene.buildIndex == 1) // Assuming Level 2 is build index 1
        {
            scoreManager.scoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
            scoreManager.timerText = GameObject.Find("TimerText").GetComponent<TMP_Text>();
            scoreManager.lifeText = GameObject.Find("LifeText").GetComponent<TMP_Text>();
        }

        //StartGame();  // Start the game once the new scene is fully loaded
    }

    private void ResetGameStatus()
    {
        lives = 3;  // Reset lives or keep the current value as needed
        scoreManager.ResetTimer();  // Reset the timer for the new level
        scoreManager.ResetScore();  // If applicable, reset the score
        scoreManager.UpdateLifeText(lives);  // Update the life text
    }

    public void AddScore(int points)
    {
        if (scoreManager != null)
        {
            scoreManager.AddScore(points);
        }
        else
        {
            Debug.LogError("ScoreManager is not assigned in GameManager.");
        }
    }

    public void ShowMainMenu()
    {
        if (mainMenuCanvas != null)
        {
            mainMenuCanvas.SetActive(true);
            Debug.Log("MainMenuCanvas set active");
            gameOverCanvas.SetActive(false);
            Debug.Log("GameOverCanvas set inactive");

            // Find the Panel inside the mainMenuCanvas
            GameObject panel = mainMenuCanvas.transform.Find("Panel").gameObject;

            if (panel != null)
            {
                Debug.Log("Panel found inside MainMenuCanvas");

                // Find the StartButton inside the Panel and set it up
                Button startButton = panel.transform.Find("StartButton").GetComponent<Button>();
                if (startButton != null)
                {
                    Debug.Log("StartButton found inside Panel");
                    startButton.interactable = true;
                    startButton.onClick.RemoveAllListeners();
                    startButton.onClick.AddListener(StartGame);
                    Debug.Log("StartGame method assigned to StartButton");
                }
                else
                {
                    Debug.LogError("StartButton not found inside the Panel.");
                }
            }
            else
            {
                Debug.LogError("Panel not found inside MainMenuCanvas.");
            }
        }

        Time.timeScale = 0; // Pause the game at the start
    }

    public void ShowGameOverMenu()
    {

        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true); // Show the GameOverCanvas

            // Find the RestartButton inside the GameOverCanvas and link it to StartGame
            Button restartButton = gameOverCanvas.transform.Find("Panel/RestartButton").GetComponent<Button>();
            if (restartButton != null)
            {
                restartButton.onClick.RemoveAllListeners();
                restartButton.onClick.AddListener(StartGame);
                Debug.Log("RestartButton is set up to call StartGame.");
            }
            else
            {
                Debug.LogError("RestartButton not found inside the GameOverCanvas.");
            }
        }
        else
        {
            Debug.LogError("GameOverCanvas is not assigned.");
        }
    }

    private void OnSceneLoadedForMainMenu(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoadedForMainMenu; // Remove the listener after it's done
        Debug.Log("Scene loaded: " + scene.name + scene.buildIndex);

        if (scene.buildIndex == 0) // Assuming Level 0 is the main menu
        {
            ShowMainMenu();
        }
    }
}
