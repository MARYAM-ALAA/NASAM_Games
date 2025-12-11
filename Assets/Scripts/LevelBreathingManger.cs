using UnityEngine;

public class LevelBreathingManager : MonoBehaviour
{
    public enum BreathPhase
    {
        None,
        Inhale,
        Hold,
        Exhale
    }

    [Header("Breathing Durations (seconds)")]
    [SerializeField] private float inhaleDuration = 3f;
    [SerializeField] private float holdDuration = 3f;
    [SerializeField] private float exhaleDuration = 3f;

    [Header("References")]
    [SerializeField] private BreathInput breathInput;      // سكربت المايك
    [SerializeField] private EnemySpawner[] spawners;      // LeftSpawner و RightSpawner

    [Header("Level Settings")]
    [SerializeField] private int totalEnemiesInLevel = 3;

    [Header("Debug")]
    [SerializeField] private bool debugLog = false;

    // -------- Public properties للـ UI / لوجيك تاني --------
    public float PhaseProgress01 { get; private set; } = 0f;  // 0→1 لكل phase
    public int SpawnedEnemiesCount { get; private set; } = 0;
    public bool LevelFinished { get; private set; } = false;
    public BreathPhase CurrentPhase => _currentPhase;

    public float CurrentPhaseElapsed => _phaseTimer;
    public float CurrentPhaseDuration
    {
        get
        {
            switch (_currentPhase)
            {
                case BreathPhase.Inhale: return inhaleDuration;
                case BreathPhase.Hold: return holdDuration;
                case BreathPhase.Exhale: return exhaleDuration;
                default: return 0f;
            }
        }
    }

    // -------- Private fields --------
    private BreathPhase _currentPhase = BreathPhase.None;
    private float _phaseTimer = 0f;
    private EnemyController _currentEnemy;

    private void Start()
    {
        StartNextEnemy();
    }

    private void Update()
    {
        if (LevelFinished)
            return;

        // لو العدو الحالي مات بالنار → نطلع اللي بعده (لو لسه في أعداء)
        if (_currentEnemy == null)
        {
            if (SpawnedEnemiesCount >= totalEnemiesInLevel)
            {
                LevelFinished = true;
                if (debugLog) Debug.Log("Level finished! All enemies spawned and defeated.");
                return;
            }

            StartNextEnemy();
            return;
        }

        // نحدّث التايمر بتاع التنفّس
        UpdateBreathingPhase();
    }

    private void StartNextEnemy()
    {
        if (SpawnedEnemiesCount >= totalEnemiesInLevel)
        {
            LevelFinished = true;
            if (debugLog) Debug.Log("Level finished! No more enemies to spawn.");
            return;
        }

        if (spawners == null || spawners.Length == 0)
        {
            Debug.LogWarning("LevelBreathingManager: No spawners assigned!");
            return;
        }

        EnemySpawner chosen = spawners[Random.Range(0, spawners.Length)];
        _currentEnemy = chosen.SpawnOne();
        SpawnedEnemiesCount++;

        // نرجّع دورة التنفّس لأولها: Inhale
        _currentPhase = BreathPhase.Inhale;
        _phaseTimer = 0f;
        PhaseProgress01 = 0f;

        if (debugLog)
            Debug.Log($"Spawned enemy {SpawnedEnemiesCount}/{totalEnemiesInLevel} from {chosen.name}. Phase: INHALE");
    }

    private void UpdateBreathingPhase()
    {
        if (breathInput == null)
            return;

        switch (_currentPhase)
        {
            case BreathPhase.Inhale:
                UpdateInhale();
                break;

            case BreathPhase.Hold:
                UpdateHold();
                break;

            case BreathPhase.Exhale:
                UpdateExhale();
                break;
        }
    }

    private void UpdateInhale()
    {
        // الطفل لازم يكون بيشهيق عشان نحسب الوقت
        if (breathInput.IsInhaling)
        {
            _phaseTimer += Time.deltaTime;
        }

        PhaseProgress01 = Mathf.Clamp01(_phaseTimer / inhaleDuration);

        if (_phaseTimer >= inhaleDuration)
        {
            _currentPhase = BreathPhase.Hold;
            _phaseTimer = 0f;
            PhaseProgress01 = 0f;

            if (debugLog)
                Debug.Log("Inhale complete → HOLD");
        }
    }

    private void UpdateHold()
    {
        // حبس النفس: مش محتاج input
        _phaseTimer += Time.deltaTime;
        PhaseProgress01 = Mathf.Clamp01(_phaseTimer / holdDuration);

        if (_phaseTimer >= holdDuration)
        {
            _currentPhase = BreathPhase.Exhale;
            _phaseTimer = 0f;
            PhaseProgress01 = 0f;

            if (debugLog)
                Debug.Log("Hold complete → EXHALE");
        }
    }

    private void UpdateExhale()
    {
        // الزفير: الطفل لازم يكون بيزفر (IsExhaling)
        if (breathInput.IsExhaling)
        {
            _phaseTimer += Time.deltaTime;
        }

        PhaseProgress01 = Mathf.Clamp01(_phaseTimer / exhaleDuration);

        if (_phaseTimer >= exhaleDuration)
        {
            // دورة التنفّس للعدو ده خلصت
            // لكن العدو لسه عايش لحد ما النار تقتله
            _currentPhase = BreathPhase.Inhale;
            _phaseTimer = 0f;
            PhaseProgress01 = 0f;

            if (debugLog)
                Debug.Log("Exhale complete → back to INHALE (same enemy still alive)");
        }
    }
}
