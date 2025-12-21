using UnityEngine;
using System.Collections;

public class BallOnTree : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasFallen = false;
    private bool hasPlayedSound = false;

    public AudioSource audioSource;
    public float rollSpeed = 2f; // سرعة التدحرج
    public float rollTime = 3f;  // مدة التدحرج قبل التوقف

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // تمنع الكورة من السقوط في البداية
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasFallen && collision.gameObject.CompareTag("Player"))
        {
            DropBall();
        }
    }

    void DropBall()
    {
        hasFallen = true;
        rb.isKinematic = false; // الكورة تبدأ تسقط

        // نبدأ التدحرج بعد السقوط
        StartCoroutine(RollAndStop());
    }

    IEnumerator RollAndStop()
    {
        float timer = 0f;

        // ندي الكورة سرعة أفقية للتدحرج
        rb.linearVelocity = new Vector2(rollSpeed, rb.linearVelocity.y);

        while (timer < rollTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // نوقف الكورة
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f; // لو في دوران
        rb.isKinematic = true; // تمنع أي حركة بعد كده

        // شغل الصوت بعد ما الكورة توقف
        PlayWinSound();
    }

    void PlayWinSound()
    {
        if (!hasPlayedSound && audioSource)
        {
            hasPlayedSound = true;
            audioSource.Play();
        }
    }
}
