using UnityEngine;
using UnityEngine.UI;

public class GameLoopUIUpdate : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject timerBar = null;
    [SerializeField] private Text timerText = null;

    public void UpdateTimerUI(object sender, TimerUpdateUIEventArgs e)
    {
        timerText.text = e.CurrentTime.ToString("0.0");
        timerBar.transform.localScale = new Vector3(e.CurrentTime / e.AllTime, timerBar.transform.localScale.y, 1);
    }

    public void UpdateLoseUI()
    {

    }
}
