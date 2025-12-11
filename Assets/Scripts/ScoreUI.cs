using UnityEngine;
using UnityEngine.UI;

public class ScoreIconFader : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image iconImage;

    [Header("Progress Settings")]
    [SerializeField] private int requiredKills = 5;
    [SerializeField] private float minAlpha = 0.15f;
    [SerializeField] private float maxAlpha = 1f;

    private void Awake()
    {
        if (iconImage == null)
            iconImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        ScoreManager.OnScoreChanged += HandleScoreChanged;

        // ✅ نعمل تحديث مبدئي بالـ Score الحالي حتى لو لسه 0
        if (ScoreManager.Instance != null)
        {
            HandleScoreChanged(ScoreManager.Instance.Score);
        }
        else
        {
            // لو مفيش ScoreManager لسه، نخليها شفافة مبدئيًا
            SetAlpha(minAlpha);
        }
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreChanged -= HandleScoreChanged;
    }

    private void HandleScoreChanged(int currentScore)
    {
        float t = Mathf.Clamp01((float)currentScore / requiredKills);
        float newAlpha = Mathf.Lerp(minAlpha, maxAlpha, t);
        SetAlpha(newAlpha);
    }

    private void SetAlpha(float alpha)
    {
        if (iconImage == null) return;

        Color c = iconImage.color;
        c.a = alpha;
        iconImage.color = c;
    }
}
