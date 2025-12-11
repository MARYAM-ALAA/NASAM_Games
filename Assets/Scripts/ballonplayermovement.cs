using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 7f;    
    [SerializeField] private float verticalSpeed = 3f; 

    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("Balloon Settings")]
    [SerializeField] private Transform balloon;
    [SerializeField] private float normalBalloonSize = .6f;
    [SerializeField] private float maxBalloonSize = 1.2f;
    [SerializeField] private float balloonSmooth = 5f;
    [SerializeField] private float floatHeight = 0.5f;   

    private Rigidbody2D rb;
    private Vector2 movement;

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
        HandleBalloon();
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

        if (movement.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (movement.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void HandleBalloon()
    {
        if (balloon == null) return;

        bool isGoingUp = movement.y > 0;                         
        bool isAboveGround = transform.position.y > floatHeight; 

        
        float targetSize = (isGoingUp || isAboveGround)
                           ? maxBalloonSize
                           : normalBalloonSize; 

        balloon.localScale = Vector3.Lerp(
            balloon.localScale,
            Vector3.one * targetSize,
            Time.deltaTime * balloonSmooth
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("target"))
        {
            Destroy(collision.gameObject);
        }
    }
}
