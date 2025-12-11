using UnityEngine;
using UnityEngine.UI;

public class SimpleBreathingCycleUI : MonoBehaviour
{
    public enum Phase
    {
        Inhale,
        Hold,
        Exhale,
        Done
    }

    [Header("UI")]
    [SerializeField] private Text timerText;
    [SerializeField] private Text phaseText;

    [Header("Durations (seconds)")]
    [SerializeField] private float inhaleDuration = 3f;
    [SerializeField] private float holdDuration = 3f;
    [SerializeField] private float exhaleDuration = 3f;

    [Header("Enemies / Cycles")]
    [SerializeField] private int totalCycles = 3;   // نفس عدد enemies

    private float _timer;
    private Phase _currentPhase = Phase.Inhale;
    private int _currentCycle = 0;

    private void Start()
    {
        if (timerText == null || phaseText == null)
        {
            Debug.LogError("Assign timerText & phaseText in inspector!");
            enabled = false;
            return;
        }

        StartPhase(Phase.Inhale);
    }

    private void Update()
    {
        if (_currentPhase == Phase.Done)
            return;

        _timer -= Time.deltaTime;
        if (_timer < 0f)
        {
            _timer = 0f;
            GoToNextPhase();
        }

        // UI تحديث
        timerText.text = $"{_timer:0.0}s";
        phaseText.text = GetPhaseLabel(_currentPhase);
    }

    private void StartPhase(Phase newPhase)
    {
        _currentPhase = newPhase;

        switch (newPhase)
        {
            case Phase.Inhale:
                _timer = inhaleDuration;
                break;

            case Phase.Hold:
                _timer = holdDuration;
                break;

            case Phase.Exhale:
                _timer = exhaleDuration;
                break;

            case Phase.Done:
                timerText.text = "0.0s";
                phaseText.text = "FINISHED";
                return;
        }
    }

    private void GoToNextPhase()
    {
        switch (_currentPhase)
        {
            case Phase.Inhale:
                StartPhase(Phase.Hold);
                break;

            case Phase.Hold:
                StartPhase(Phase.Exhale);
                break;

            case Phase.Exhale:
                // دي دورة كاملة لعدو واحد
                _currentCycle++;

                if (_currentCycle >= totalCycles)
                {
                    StartPhase(Phase.Done);
                }
                else
                {
                    StartPhase(Phase.Inhale); // نبدأ دورة جديدة
                }
                break;
        }
    }

    private string GetPhaseLabel(Phase phase)
    {
        switch (phase)
        {
            case Phase.Inhale: return "Inhale";
            case Phase.Hold: return "Hold";
            case Phase.Exhale: return "Exhale";
            default: return "";
        }
    }
}
