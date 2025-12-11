using UnityEngine;
using UnityEngine.UI;

public class BreathingTimerUI : MonoBehaviour
{
    [SerializeField] private LevelBreathingManager breathingManager;
    [SerializeField] private Text timerText;

    [Header("Labels")]
    [SerializeField] private string inhaleLabel = "Inhale";
    [SerializeField] private string holdLabel = "Hold";
    [SerializeField] private string exhaleLabel = "Exhale";
    [SerializeField] private string finishedLabel = "Done";

    private void Awake()
    {
        if (timerText == null)
            timerText = GetComponent<Text>();
    }

    private void Update()
    {
        if (breathingManager == null || timerText == null)
            return;

        if (breathingManager.LevelFinished)
        {
            timerText.text = finishedLabel;
            return;
        }

        var phase = breathingManager.CurrentPhase;
        float elapsed = breathingManager.CurrentPhaseElapsed;
        float duration = breathingManager.CurrentPhaseDuration;

        if (duration <= 0f)
        {
            timerText.text = "";
            return;
        }

        float remaining = Mathf.Max(0f, duration - elapsed);
        string phaseLabel = GetPhaseLabel(phase);

        // مثال: "Inhale: 2.7s"
        timerText.text = $"{phaseLabel}: {remaining:0.0}s";
    }

    private string GetPhaseLabel(LevelBreathingManager.BreathPhase phase)
    {
        switch (phase)
        {
            case LevelBreathingManager.BreathPhase.Inhale:
                return inhaleLabel;
            case LevelBreathingManager.BreathPhase.Hold:
                return holdLabel;
            case LevelBreathingManager.BreathPhase.Exhale:
                return exhaleLabel;
            default:
                return "";
        }
    }
}
