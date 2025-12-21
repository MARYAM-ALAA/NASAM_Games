using UnityEngine;

public class BreathGate : MonoBehaviour
{
    public int pressesNeeded = 4;
    private int currentPresses = 0;

    public Collider2D hole; // الحائط الأصلي
    private bool playerInside = false;
    private bool holeOpened = false; // flag جديد

    [Header("Audio")]
    public AudioSource inhaleAudio; // ضع هنا صوت Inhale

    void Update()
    {
        if (!playerInside || holeOpened) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentPresses++;
            Debug.Log("Inhale Presses: " + currentPresses);

            if (currentPresses >= pressesNeeded)
            {
                OpenHole();
            }
        }
    }

    void OpenHole()
    {
        hole.enabled = false; // فتح الحائط
        holeOpened = true;    // العلم أن الحائط اتفتح
       

        this.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !holeOpened)
        {
            playerInside = true;
            currentPresses = 0;

            // شغل صوت Inhale مرة واحدة فقط
            if (inhaleAudio != null && !inhaleAudio.isPlaying)
            {
                inhaleAudio.Play();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = false;
            currentPresses = 0;
        }
    }
}
