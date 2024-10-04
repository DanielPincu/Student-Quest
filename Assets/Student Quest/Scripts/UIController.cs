using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject canvasGame; // The main canvas for the game
    [SerializeField]
    private GameObject HUD; // The HUD that displays live info
    [SerializeField]
    private GameObject pauseMenu; // The UI panel for the pause menu

    [SerializeField]
    private Text txtLifes; // UI text for displaying lives
    [SerializeField]
    private Text txtTime; // UI text for displaying time left
    [SerializeField]
    private Text txtCoins; // UI text for displaying coins
    [SerializeField]
    private Image fillCapsule; // Fill image for power-up capsule

    [Space(10)]
    [SerializeField]
    private GameObject EndGameHolder; // The end game UI holder

    [SerializeField]
    private Text txtAllCoins; // Total coins text at end
    [SerializeField]
    private Text txtAllRedCoins; // Total red coins text at end
    [SerializeField]
    private Text txtAllBricks; // Total bricks text at end
    [SerializeField]
    private Text txtAllGoldenBlocks; // Total golden blocks text at end
    [SerializeField]
    private Text txtAllTime; // Total time text at end
    [SerializeField]
    private Text txtAllPerc; // Percentage text at end

    [Header("Events")]
    [SerializeField] private UnityEvent onGameNotStarted; // Event for when the game hasn't started
    [SerializeField] private UnityEvent onGameStarted; // Event for when the game has started
    [SerializeField] private UnityEvent onStartGame; // Event for starting the game

    private static bool gameStarted = false; // Tracks if the game has started
    private static bool isPaused = false; // Static variable to track pause state
    public static UIController instance; // Singleton instance for easy access

    private void Awake()
    {
        instance = this; // Set the singleton instance
        Cursor.visible = false; // Hide the cursor in-game

        txtCoins.text = "0"; // Initialize coin text
        fillCapsule.fillAmount = 0; // Reset fill amount

        canvasGame.SetActive(true); // Activate the game canvas
        pauseMenu.SetActive(false); // Ensure pause menu is hidden initially
    }

    private void Start()
    {
        if (!gameStarted)
        {
            onGameNotStarted.Invoke(); // Invoke event if game hasn't started
        }
        else
        {
            onGameStarted.Invoke(); // Invoke event if game has started
        }
    }

    private void Update()
    {
        if (!gameStarted)
        {
            if (Input.anyKeyDown) // Listen for any key press to start
            {
                onStartGame.Invoke(); // Invoke start game event
                enabled = false; // Disable the script after starting
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) // Check for Escape key to toggle pause
        {
            TogglePause(); // Toggle pause state when the key is pressed
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused; // Toggle the pause state
        PauseGame(isPaused); // Call PauseGame with the new state
    }

    public void PauseGame(bool value)
    {
        if (value)
        {
            Time.timeScale = 0; // Pause the game
            Cursor.visible = true; // Show the cursor
            pauseMenu.SetActive(true); // Show pause menu
        }
        else
        {
            Time.timeScale = 1; // Resume the game
            Cursor.visible = false; // Hide the cursor
            pauseMenu.SetActive(false); // Hide pause menu
        }
    }

    public void StartGame()
    {
        gameStarted = true; // Set game started flag
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the application
    }

    public void AddCoin(int value)
    {
        txtCoins.text = value.ToString(); // Update coin text
    }

    public void SetTime(int value)
    {
        txtTime.text = value.ToString(); // Update time text
    }

    public void SetLife(int value)
    {
        txtLifes.text = value.ToString(); // Update life text
    }

    public void Fill(float perc)
    {
        fillCapsule.fillAmount = perc; // Set power-up capsule fill amount
    }

    public void ShowEndGame()
    {
        StartCoroutine(EndSequence()); // Start end game sequence
    }

    private IEnumerator EndSequence()
    {
        yield return null; // Wait for a frame

        // Clear the text fields to prevent any display of previous values
        txtAllRedCoins.text = "";
        txtAllCoins.text = "";
        txtAllBricks.text = "";
        txtAllGoldenBlocks.text = "";
        txtAllTime.text = "";
        txtAllPerc.text = "";

        HUD.SetActive(false); // Hide HUD
        GameManager.instance.PowerUpCamera(); // Activate end game camera effect

        yield return new WaitForSeconds(1); // Wait before showing end game UI

        Cursor.visible = true; // Show the cursor
        GameManager.instance.EndGame(); // Trigger end game
        GameManager.instance.StopPlayer(); // Stop the player actions
        EndGameHolder.SetActive(true); // Show end game UI
    }
}
