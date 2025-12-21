using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class BallOnTree : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasFallen = false;
    private bool hasPlayedSound = false;

    public AudioSource audioSource;
    public float rollSpeed = 2f; // سرعة التدحرج
    public float rollTime = 3f;  // مدة التدحرج قبل التوقف

    [Header("Winner Scene")]
    public string winnerSceneName = "winnerballon";
    public float delayBeforeWinScene = 2f;


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
        rb.angularVelocity = 0f;
        rb.isKinematic = true;

        // شغل صوت الفوز
        PlayWinSound();

        // استنى شوية وبعدين افتح Winner Scene
        yield return new WaitForSeconds(delayBeforeWinScene);
        SceneManager.LoadScene(winnerSceneName);
    }


    void PlayWinSound()
    {
        if (!hasPlayedSound && audioSource)
        {
            hasPlayedSound = true;

            // خلي الصوت يفضل بعد تغيير المشهد
            audioSource.transform.parent = null; // نفصل الصوت عن الكورة
            DontDestroyOnLoad(audioSource.gameObject);

            audioSource.Play();
        }
    }

}
