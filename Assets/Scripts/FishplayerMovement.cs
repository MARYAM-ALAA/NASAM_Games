using UnityEngine;

public class FishplayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;
    public Transform leftLimit;
    public Transform rightLimit;
    public Transform topLimit;
    public Transform bottomLimit;
    public GameObject underwater;
    public Camera mainCamera;
    public AudioSource fishSound;
    public Transform underwaterBackground;

    private Vector2 movement;
    private Vector3 prevFishPos;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        prevFishPos = transform.position;
        if (underwater != null) underwater.SetActive(false);
    }

    void Update()
    {
        float x = 0f;
        float y = 0f;
        //

        if (Input.GetKey(KeyCode.RightArrow)) x = speed;
        if (Input.GetKey(KeyCode.LeftArrow)) x = -speed;
        if (Input.GetKey(KeyCode.UpArrow)) y = speed;
        if (Input.GetKey(KeyCode.DownArrow)) y = -speed;

        movement = new Vector2(x, y);

        if (x > 0) transform.localScale = new Vector3(2.5f, 2.5f, 1);
        else if (x < 0) transform.localScale = new Vector3(-2.5f, 2.5f, 1);

        if (underwater != null)
            underwater.SetActive(transform.position.y < bottomLimit.position.y + 1f);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement;

        float clampedX = Mathf.Clamp(rb.position.x, leftLimit.position.x, rightLimit.position.x);
        float clampedY = Mathf.Clamp(rb.position.y, bottomLimit.position.y, topLimit.position.y);
        Vector3 currentPos = new Vector3(clampedX, clampedY, transform.position.z);
        rb.position = new Vector2(currentPos.x, currentPos.y);

       
        if (mainCamera != null)
        {
            mainCamera.transform.position = new Vector3(currentPos.x, currentPos.y, mainCamera.transform.position.z);
        }

     
        if (underwaterBackground != null)
        {
            Vector3 delta = currentPos - prevFishPos;
            underwaterBackground.position += new Vector3(delta.x, delta.y, 0);
        }

        prevFishPos = currentPos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("target"))
        {
            Destroy(collision.gameObject);
            Score.scorevalue += 1;
            if (fishSound != null) fishSound.Play();
        }
    }
}
