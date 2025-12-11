using UnityEngine;
using UnityEngine.Audio;

public class Dragon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform mouthPoint;
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private Transform visualRoot;
    [SerializeField] private Animator dragonAnimator;
    [SerializeField] private BreathInput breathInput;

    [Header("Fire Settings")]
    [SerializeField] private float fireSpeed = 8f;
    [SerializeField] private float fireCooldown = 0.6f;
    [SerializeField] private float maxChargeTime = 2f;

    [Header("Enemy Detection")]
    [SerializeField] private string enemyTag = "Enemy"; // التاج بتاع الأعداء
    [SerializeField] private float detectionRange = 20f; // أقصى مسافة يبص فيها على عدو

    [Header("Source Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip exhaleSFX;


    private float _lastFireTime;
    private int _facingDirection = 1;
    private float _currentCharge;

    private void Update()
    {
        HandleAutoLookAtEnemies(); // يبص على الأعداء
        HandleBreathLogic();       // شهيق/زفير + أنيميشن + نار
    }

    private void HandleAutoLookAtEnemies()
    {
        // ندوّر على كل الأعداء في السين
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        if (enemies.Length == 0)
            return;

        GameObject nearestEnemy = null;
        float nearestDist = Mathf.Infinity;

        Vector3 myPos = transform.position;

        foreach (GameObject enemy in enemies)
        {
            if (!enemy.activeInHierarchy) continue;

            float dist = Vector3.Distance(myPos, enemy.transform.position);

            if (dist < nearestDist && dist <= detectionRange)
            {
                nearestDist = dist;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy == null)
            return;

        // نحدد العدو على يمين ولا شمال
        if (nearestEnemy.transform.position.x > myPos.x + 0.01f)
        {
            // العدو على اليمين
            _facingDirection = 1;
            FlipVisual(true);
        }
        else if (nearestEnemy.transform.position.x < myPos.x - 0.01f)
        {
            // العدو على الشمال
            _facingDirection = -1;
            FlipVisual(false);
        }
    }

    private void HandleBreathLogic()
    {
        if (breathInput == null)
            return;

        bool inhaling = breathInput.IsInhaling;

        // نبلّغ الأنيميتر إن في شهيق ولا لا
        if (dragonAnimator != null)
        {
            dragonAnimator.SetBool("Inhale", inhaling);
        }

        // شحن أثناء الشهيق
        if (inhaling)
        {
            _currentCharge += Time.deltaTime;
            _currentCharge = Mathf.Clamp(_currentCharge, 0f, maxChargeTime);

            if (dragonAnimator != null)
            {
                float charge01 = _currentCharge / maxChargeTime;
                dragonAnimator.SetFloat("Inhale", charge01);
            }
        }
        else
        {
            // تفريغ بسيط لو وقف شهيق (اختياري)
            if (_currentCharge > 0f)
            {
                _currentCharge -= Time.deltaTime;
                _currentCharge = Mathf.Max(0f, _currentCharge);

                if (dragonAnimator != null)
                {
                    float charge01 = _currentCharge / maxChargeTime;
                    dragonAnimator.SetFloat("Inhale", charge01);
                }
            }
        }

        // أول ما الزفير يبدأ
        if (breathInput.ExhaleJustStarted)
        {
            if (dragonAnimator != null)
            {
                dragonAnimator.SetTrigger("Exhale");
                audioSource.PlayOneShot(exhaleSFX);

            }

            TryFire();

            _currentCharge = 0f;
            if (dragonAnimator != null)
            {
                dragonAnimator.SetFloat("Inhale", 0f);
            }
        }
    }

    private void TryFire()
    {
        if (Time.time - _lastFireTime < fireCooldown)
            return;

        _lastFireTime = Time.time;

        GameObject fireInstance = Instantiate(
            firePrefab,
            mouthPoint.position,
            Quaternion.identity
        );

        // 1) نحدد اتجاه شكل النار
        // لو السبرايت بتاع النار معمول facing يمين by default
        Vector3 fireScale = fireInstance.transform.localScale;
        fireScale.x = Mathf.Abs(fireScale.x) * (_facingDirection == 1 ? 1 : -1);
        fireInstance.transform.localScale = fireScale;

        // 2) نخليها تتحرك في نفس الاتجاه
        Rigidbody2D rb = fireInstance.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 dir = new Vector2(_facingDirection, 0f);
            rb.linearVelocity = dir * fireSpeed;
        }
    }


    private void FlipVisual(bool faceRight)
    {
        if (visualRoot == null) return;

        Vector3 scale = visualRoot.localScale;
        scale.x = Mathf.Abs(scale.x) * (faceRight ? 1 : -1);
        visualRoot.localScale = scale;
    }
}
