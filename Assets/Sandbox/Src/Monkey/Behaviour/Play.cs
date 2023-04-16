using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    /*** Public variables ***/
    public bool verbose = false;

    public GameObject gameManager;

    public GameObject gameOverMenuUI;

    public GameObject victoryMenuUI;

    public GameObject scoreGameObject;

    [Header("World")]
    public Vector2 originSpawn = Vector2.zero;

    /*** Private variables ***/
    private GameHandler gameHandler;

    private Score score;

    private PregamePhase pregamePhase;

    private AutoMovement autoMovement;

    private Moveset moveset;

    private MonkeyAnimation monkeyAnimation;

    private UnityEvent monkeyTouchGroundEvent;

    private uint nbWallJumps;

    void Start()
    {
        /* Game Management */
        this.gameHandler = this.gameManager.GetComponent<GameHandler>();
        this.score = this.scoreGameObject.GetComponent<Score>();

        /** Monkey gameplay scripts **/
        this.pregamePhase = this.GetComponent<PregamePhase>();
        this.autoMovement = this.GetComponent<AutoMovement>();
        this.moveset = this.GetComponent<Moveset>();
        this.monkeyAnimation = this.GetComponent<MonkeyAnimation>();

        /** Monkey gameplay variables **/
        this.UpdateNbWallJumps(0);
        this.gameHandler.gameState = GameState.PREGAME;

        /** Monkey events management **/
        /* Player clicks on Monkey during Pregame phase, hence starting the game */
        this.pregamePhase.SetPlayerClickEvent(StartGame);

        /* Monkey collides a non-ground environment during Pregame phase, hence respawning */
        this.pregamePhase.SetNotOnGroundEvent(RespawnMonkey);

        /* Monkey collides ground environment during In-Game phase, hence losing the game */
        this.autoMovement.SetMonkeyFallsEvent(Lose);

        /* Monkey reaches level end during In-Game phase, hence winning the game */
        this.autoMovement.SetMonkeyWinsEvent(Win);

        /* Monkey wall jumps, hence incrementing score */
        this
            .autoMovement
            .SetMonkeyWallJumpsEvent(() =>
                this.UpdateNbWallJumps(this.nbWallJumps + 1));

        /* Monkey reaches maximum score, hence losing the game */
        this.score.SetReachMaxScoreEventEvent(Lose);

        /** Monkey origin spawn **/
        this.RespawnMonkey();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.gameHandler.gameState == GameState.INGAME)
        {
            this.autoMovement.ManageCollision(collision);
        }
        else if (this.gameHandler.gameState == GameState.PREGAME)
        {
            this.pregamePhase.ManageCollision(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayingArea")
        {
            if (this.verbose) Debug.Log("[DEBUG] Monkey left playing area.");
            this.moveset.StopForces();
            this.RespawnMonkey();
        }
    }

    public void RespawnMonkey()
    {
        if (this.verbose) Debug.Log("[DEBUG] Monkey respawns to origin.");
        this.monkeyAnimation.MonkeyStands();
        this.transform.position = originSpawn;
        this.moveset.ResetOrientation();
        this.pregamePhase.StopDragging();
    }

    private void StartGame()
    {
        if (this.verbose) Debug.Log("[DEBUG] Game starts.");

        /* Prevent the player from dragging Monkey anymore */
        this.pregamePhase.enabled = false;

        this.autoMovement.StartAutomovement();
        this.gameHandler.gameState = GameState.INGAME;
    }

    private void Win()
    {
        if (this.verbose) Debug.Log("[DEBUG] Game won.");

        // TODO
        // Play Victory animation
        this.gameHandler.gameState = GameState.IDLE;

        /* Show Victory menu */
        this.victoryMenuUI.SetActive(true);
        this.gameHandler.isMenuOpen = true;
    }

    private void Lose()
    {
        if (this.verbose) Debug.Log("[DEBUG] Game lost.");

        // TODO
        // Play defeat animation
        this.monkeyAnimation.MonkeyFalls();
        this.gameHandler.gameState = GameState.IDLE;

        /* Show Game Over menu */
        this.gameOverMenuUI.SetActive(true);
        this.gameHandler.isMenuOpen = true;
    }

    public void Retry()
    {
        this.gameHandler.gameState = GameState.PREGAME;
        this.UpdateNbWallJumps(0);

        /* Respawn to current level origin */
        this.moveset.StopForces();
        this.RespawnMonkey();

        /* Allow the player to drag Monkey again */
        this.pregamePhase.enabled = true;

        /* Hide Win/Lose menu */
        this.gameOverMenuUI.SetActive(false);
        this.victoryMenuUI.SetActive(false);
        this.gameHandler.isMenuOpen = false;
    }

    private void UpdateNbWallJumps(uint newNbWallJumps)
    {
        this.nbWallJumps = newNbWallJumps;
        this.score.UpdateScore(newNbWallJumps);
    }
}
