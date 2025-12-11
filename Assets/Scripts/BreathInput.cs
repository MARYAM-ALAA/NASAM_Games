using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BreathInput : MonoBehaviour
{
    [Header("Mic Settings")]
    [SerializeField] private string deviceName = null;  // null = أول مايك متاح
    [SerializeField] private int sampleRate = 44100;
    [SerializeField] private int sampleWindow = 256;    // عدد السامبلز اللي هنقيس عليهم كل Frame

    [Header("Thresholds")]
    [SerializeField] private float inhaleThreshold = 0.02f; // حساسية الشهيق (قيم تقريبية - تتظبط من Inspector)
    [SerializeField] private float exhaleThreshold = 0.08f; // حساسية الزفير (بيكون أعلى)

    [Header("Timing")]
    [SerializeField] private float minInhaleTime = 0.2f; // وقت أدنى للشهيق عشان نعتبره شهيق بجد

    [Header("Sensitivity")]
    [SerializeField] private float sensitivityMultiplier = 3f;


    public bool IsInhaling { get; private set; }
    public bool ExhaleJustStarted { get; private set; }
    public bool IsExhaling { get; private set; } 


    private AudioSource _audioSource;
    private AudioClip _micClip;
    private float[] _samples;

    private enum BreathState { None, Inhale, Exhale }
    private BreathState _currentState = BreathState.None;
    private BreathState _lastState = BreathState.None;

    private float _inhaleTimer;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _samples = new float[sampleWindow];

        // نبدأ نسجّل من المايك
        if (Microphone.devices.Length == 0)
        {
            Debug.LogWarning("No microphone found!");
            enabled = false;
            return;
        }

        if (string.IsNullOrEmpty(deviceName))
        {
            deviceName = Microphone.devices[0]; // أول مايك
        }

        _micClip = Microphone.Start(deviceName, true, 10, sampleRate); // 10 ثواني لوب
        _audioSource.clip = _micClip;
        _audioSource.loop = true;
        _audioSource.mute = true;
        while (Microphone.GetPosition(deviceName) <= 0) { } // نستنى شوية لحد ما يبدأ
        _audioSource.Play();
    }

    private void Update()
    {
        ExhaleJustStarted = false;
        IsExhaling = false;

        float level = GetCurrentRMS();

        level *= sensitivityMultiplier;   // 👈 نعلّي الإشارة شوية

        // Debug لو عايزه تشوفي بعد الضرب
        //Debug.Log($"Level (after gain): {level:F5}");

        // نحدّد state مبدئية من الـ level
        BreathState newState = BreathState.None;

        if (level > exhaleThreshold)
        {
            newState = BreathState.Exhale;
        }
        else if (level > inhaleThreshold)
        {
            newState = BreathState.Inhale;
        }
        else
        {
            newState = BreathState.None;
        }

        // شوية لوجيك عشان مانخليش الصوت يتهز يخربطنا بين states
        switch (newState)
        {
            case BreathState.Inhale:
                _inhaleTimer += Time.deltaTime;
                if (_inhaleTimer < minInhaleTime)
                {
                    // لسه بادئ، ما نعتبرهوش شهيق ثابت غير بعد minInhaleTime
                    IsInhaling = false;
                    _currentState = BreathState.None;
                }
                else
                {
                    IsInhaling = true;
                    _currentState = BreathState.Inhale;
                }
                break;

            case BreathState.Exhale:
                IsInhaling = false;
                IsExhaling = true;        
                _inhaleTimer = 0f;
                _currentState = BreathState.Exhale;
                break;


            default:
                IsInhaling = false;
                _inhaleTimer = 0f;
                _currentState = BreathState.None;
                break;
        }

        // ExhaleJustStarted = أول Frame يدخل فيه Exhale
        if (_currentState == BreathState.Exhale && _lastState != BreathState.Exhale)
        {
            ExhaleJustStarted = true;
        }

        _lastState = _currentState;

 


    }

    private float GetCurrentRMS()
    {
        if (_micClip == null) return 0f;

        int micPos = Microphone.GetPosition(deviceName) - sampleWindow;
        if (micPos < 0) return 0f;

        _micClip.GetData(_samples, micPos);

        // RMS = Root Mean Square = متوسط الطاقة
        float sum = 0f;
        for (int i = 0; i < sampleWindow; i++)
        {
            float s = _samples[i];
            sum += s * s;
        }

        return Mathf.Sqrt(sum / sampleWindow);
    }
}
