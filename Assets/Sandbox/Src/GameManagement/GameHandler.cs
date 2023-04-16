using UnityEngine;

public class GameHandler : MonoBehaviour
{
    /*** Public variables ***/
    public bool isMenuOpen = false;
    public GameState gameState = GameState.IDLE;

    [Header("Level score")]
    public GameObject scoreGameObject;
    public uint maxScore = 100;

    public uint goldScore = 30;

    public uint silverScore = 50;

    public uint bronzeScore = 80;

    /*** Private variables ***/

    private void Start() {
        /** Initialize global level variables */
        /* Scoring system */
        Score score = this.scoreGameObject.GetComponent<Score>();
        score.InitializeScore(bronzeScore, silverScore, goldScore, maxScore);
    }
}
