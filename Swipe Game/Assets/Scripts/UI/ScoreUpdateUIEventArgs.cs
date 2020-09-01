using UnityEngine;

public class ScoreUpdateUIEventArgs : MonoBehaviour
{
    public int Score { get; }
    public int BestScore { get; }
    public ScoreUpdateUIEventArgs(int score, int best = 0)
    {
        Score = score;
        BestScore = best;
    }
}
