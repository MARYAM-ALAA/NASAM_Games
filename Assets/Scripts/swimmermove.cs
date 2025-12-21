using UnityEngine;

public class swimmermove : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); 
        float moveY = Input.GetAxisRaw("Vertical");  

        movement = new Vector2(moveX, moveY).normalized;
        if (moveX < 0)
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f); // شمال
        }
        else if (moveX > 0)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);  //  يمين
        }
    }
    void FixedUpdate()
    {
        rb.velocity = movement * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "targets")
        {
            Destroy(collision.gameObject);
           // coinscore.scorevalue += 10;
        }
    }
}
