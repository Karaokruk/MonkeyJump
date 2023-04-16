using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    /*** Public variables ***/
    public Text currentScoreText;

    public Text maxScoreText;

    /*** Private variables ***/
    private uint bronzeScore;
    private uint silverScore;
    private uint goldScore;
    private uint maxScore;
    private UnityEvent reachMaxScoreEvent = new UnityEvent();

    public void InitializeScore(uint bronzeScore, uint silverScore, uint goldScore, uint maxScore)
    {
        /** Initialize score variables **/
        this.bronzeScore = bronzeScore;
        this.silverScore = silverScore;
        this.goldScore = goldScore;
        this.maxScore = maxScore;

        /** Initialize text variables **/
        this.currentScoreText.text = "0";
        this.currentScoreText.color = Color.yellow;
        this.maxScoreText.text = "/" + maxScore.ToString();
        this.maxScoreText.color = Color.black;
    }

    public void UpdateScore(uint score)
    {
        if (score > maxScore)
        {
            this.reachMaxScoreEvent.Invoke();
        }
        this.currentScoreText.text = score.ToString();
        this.currentScoreText.color = this.ScoreToColor(score);
    }

    private Color ScoreToColor(uint score)
    {
        Color color;

        if (score <= this.goldScore)
        {
            color = Color.yellow;
        }
        else if (score > this.goldScore && score <= this.silverScore)
        {
            color = Color.grey;
        }
        else if (score > this.silverScore && score <= this.bronzeScore)
        {
            color = Color.red;
        }
        else
        {
            color = Color.black;
        }

        return color;
    }

    public void SetReachMaxScoreEventEvent(UnityAction call)
    {
        this.reachMaxScoreEvent.AddListener(call);
    }
}
