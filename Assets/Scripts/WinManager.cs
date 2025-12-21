using UnityEngine;

public class WinManager : MonoBehaviour
{
    public GameObject winCanvas;
    public AudioSource winSound;

    void Start()
    {
        Time.timeScale = 1f;
    }

    public void ShowWinScreen()
    {
        winCanvas.SetActive(true);
        winSound.Play();
        Time.timeScale = 0f;
    }
}
