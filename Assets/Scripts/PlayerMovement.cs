using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 7f;
    [SerializeField] private float verticalSpeed = 3f;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("Balloon Settings")]
    [SerializeField] private Transform balloon;
    [SerializeField] private float normalBalloonSize = 0.6f;
    [SerializeField] private float maxBalloonSize = 9f;
    [SerializeField] private float balloonIncrease = 1.5f;

    [Header("Sounds")]
    public AudioSource appleSound;
    public AudioSource holeAppleSound;
    public AudioSource exhaleAudio;

    [Header("Flying Settings")]
    public Transform treePosition;
    public float flySpeed = 5f;

    [Header("Breath Settings")]
    public int holdBreathPresses = 7;
    public int exhalePresses = 8;

    private Rigidbody2D rb;
    private Vector2 movement;

    // حالات التنفس والطيران
    private bool isHoldingBreath = false;
    private bool isExhaling = false;
    private bool isFlying = false;
    private int currentPresses = 0;

    // Flags
    private bool holeOpened = false;
    private bool playerInsideHole = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (animator == null)
            animator = GetComponent<Animator>();
        if (balloon != null)
            balloon.localScale = Vector3.one * normalBalloonSize;
    }

    void Update()
    {
        HandleMovementInput();
        HandleAnimationAndFlip();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isHoldingBreath)
            {
                currentPresses++;
                Debug.Log("Hold Breath Presses: " + currentPresses);

                if (currentPresses >= holdBreathPresses)
                {
                    isHoldingBreath = false;
                    currentPresses = 0;
                    StartExhalePhase();
                }
            }
            else if (isExhaling)
            {
                currentPresses++;
                Debug.Log("Exhale Presses: " + currentPresses);

                if (balloon != null)
                {
                    float newSize = Mathf.Min(maxBalloonSize, balloon.localScale.x + balloonIncrease);
                    balloon.localScale = new Vector3(newSize, newSize, newSize);
                }

                if (currentPresses >= exhalePresses)
                {
                    isExhaling = false;
                    currentPresses = 0;
                    StartFlying();
                }
            }
        }

        if (isFlying)
        {
            transform.position = Vector3.MoveTowards(transform.position, treePosition.position, flySpeed * Time.deltaTime);

        }

    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void HandleMovementInput()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        movement = new Vector2(inputX * speed, inputY * verticalSpeed) * Time.fixedDeltaTime;
    }

    void MovePlayer()
    {
        if (!isFlying) // أثناء الطيران، الحركة تتم بواسطة MoveTowards
            rb.MovePosition(rb.position + movement);
    }

    void HandleAnimationAndFlip()
    {
        if (movement.y > 0)
        {
            animator.SetBool("isRunning", false);
            return;
        }

        animator.SetBool("isRunning", movement.x != 0);
        transform.localScale = new Vector3(movement.x >= 0 ? 1 : -1, 1, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("target"))
        {
            Destroy(collision.gameObject);
            appleSound.Play();
        }
        else if (collision.gameObject.CompareTag("holetarget"))
        {
            Destroy(collision.gameObject);
            holeAppleSound.Play();

            if (!holeOpened)
            {
                holeOpened = true;
                playerInsideHole = true;
                currentPresses = 0;
                isHoldingBreath = true;
            }
        }

        if (isFlying && collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (ballRb != null)
                ballRb.isKinematic = false;

            // ابدأ Coroutine لتحريك اللاعب ناحية الشمال بشكل سلس
            StartCoroutine(MoveAwaySmoothly());

            isFlying = false;
        }



    }

    void StartExhalePhase()
    {
        isExhaling = true;
        currentPresses = 0;
        if (exhaleAudio != null && !exhaleAudio.isPlaying)
            exhaleAudio.Play();
    }

    void StartFlying()
    {
        isFlying = true;
    }

    private IEnumerator MoveAwaySmoothly()
    {
        float duration = 0.5f; // مدة الحركة بالنصف ثانية
        float elapsed = 0f;
        Vector2 startPos = rb.position;
        Vector2 targetPos = startPos + Vector2.left * 5f; // 5 وحدات ناحية الشمال

        while (elapsed < duration)
        {
            rb.MovePosition(Vector2.Lerp(startPos, targetPos, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.MovePosition(targetPos); // تأكد أنه وصل تمامًا
    }

}
