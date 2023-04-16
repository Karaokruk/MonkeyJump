using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    /*** Public variables ***/
    public bool verbose = false;

    public GameObject pauseMenuUI;

    public GameObject gameManager;

    /*** Private variables ***/
    private GameHandler gameHandler;

    private static bool gameIsPaused = false;
    private GameState previousGameState = GameState.IDLE;

    void Start()
    {
        this.gameHandler = this.gameManager.GetComponent<GameHandler>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                this.Resume();
            }
            else if (!this.gameHandler.isMenuOpen)
            {
                this.Pause();
            }
        }
    }

    public void Pause()
    {
        if (this.verbose) Debug.Log("[DEBUG] Pause");
        this.SetActive(true);
        this.gameHandler.isMenuOpen = true;
        this.previousGameState = this.gameHandler.gameState; /* Save either PREGAME or INGAME GameState */
        this.gameHandler.gameState = GameState.IDLE;
    }

    public void Resume()
    {
        if (this.verbose) Debug.Log("[DEBUG] Resume");
        this.SetActive(false);
        this.gameHandler.isMenuOpen = false;
        this.gameHandler.gameState = this.previousGameState; /* Back to either PREGAME or INGAME GameState */
    }

    private void SetActive(bool value)
    {
        gameIsPaused = value;
        this.pauseMenuUI.SetActive(value);
        Time.timeScale = value ? 0.0f : 1.0f;
    }

    public void LoadMainMenu()
    {
        if (this.verbose) Debug.Log("[DEBUG] Loading main menu...");
        this.Resume();
        SceneManager.LoadScene("MainMenu");
    }
}
