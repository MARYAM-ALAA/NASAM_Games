using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform balloon;
    [SerializeField] private float normalBalloonSize = 0.6f;
    [SerializeField] private float maxBalloonSize = 1.2f;
    [SerializeField] private float balloonSmooth = 5f;

    private Rigidbody2D rb;
    private Vector2 input;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();

        if (balloon != null)
            balloon.localScale = Vector3.one * normalBalloonSize;
    }

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        animator.SetBool("isRunning", input.x != 0);

        if (input.x > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (input.x < 0) transform.localScale = new Vector3(-1, 1, 1);

        HandleBalloon();
    }

    void FixedUpdate()
    {
        Vector2 move = input.normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }

    void HandleBalloon()
    {
        if (balloon == null) return;

        float targetSize = (input.y > 0) ? maxBalloonSize : normalBalloonSize;
        balloon.localScale = Vector3.Lerp(
            balloon.localScale,
            Vector3.one * targetSize,
            Time.deltaTime * balloonSmooth
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("target"))
            Destroy(collision.gameObject);
    }
}
