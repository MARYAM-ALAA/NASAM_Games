using UnityEngine;

public class DragonHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 3;          // عدد الضربات/القلوب
    public int CurrentHealth { get; private set; }

    [Header("Visuals / Animation")]
    [SerializeField] private Animator dragonAnimator;    // Animator بتاع DragonVisual
    [SerializeField] private GameObject losePanel;       // UI تظهر لما يخسر (اختياري)

    private bool _isDead = false;

    private void Awake()
    {
        CurrentHealth = maxHealth;

        if (dragonAnimator == null)
        {
            // لو الأنيميتر قاعد على DragonVisual مش على Dragon نفسه
            dragonAnimator = GetComponentInChildren<Animator>();
        }
    }

    public void TakeDamage(int amount)
    {
        if (_isDead) return;

        CurrentHealth -= amount;
        if (CurrentHealth < 0) CurrentHealth = 0;

        Debug.Log("Dragon Health: " + CurrentHealth);

        // أنيميشن ضربة خفيفة لو عندك
        if (dragonAnimator != null)
        {
            dragonAnimator.SetTrigger("Hit");   // اعملي Trigger اسمه Hit في الـ Animator لو حابة
        }

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _isDead = true;

        Debug.Log("Dragon Died!");

        // أنيميشن الموت
        if (dragonAnimator != null)
        {
            // يا إما Bool
            dragonAnimator.SetBool("Dead", true);
            // أو Trigger لو عندك ستايت خاصة بالموت
            // dragonAnimator.SetTrigger("Die");
        }

        // نوقف تحكم التنين عشان مايضربش نار تاني
        Dragon controller = GetComponent<Dragon>();
        if (controller != null)
        {
            controller.enabled = false;
        }

        // نقدر كمان نوقف أي سكربتات تانية لو حابة (زي LevelBreathingManager مثلاً)
        // LevelBreathingManager bm = FindObjectOfType<LevelBreathingManager>();
        // if (bm != null) bm.enabled = false;

        // نظهر شاشة الخسارة لو حاطين واحدة
        if (losePanel != null)
        {
            losePanel.SetActive(true);
        }

        // ممكن بعد شوية نعمل ريستارت أو نرجع منيو
        // StartCoroutine(RestartLevelAfterDelay(2f));
    }
}
