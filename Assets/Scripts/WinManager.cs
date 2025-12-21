//using UnityEngine;

//public class WinManager : MonoBehaviour
//{
//    public GameObject winCanvas;
//    public AudioSource winSound;

//    void Start()
//    {
//        Time.timeScale = 1f;
//    }

//    public void ShowWinScreen()
//    {
//        winCanvas.SetActive(true);
//        winSound.Play();
//        Time.timeScale = 0f;
//    }
//}

using UnityEngine;

public class WinManager : MonoBehaviour
{
    public GameObject winCanvas;
    public AudioSource winSound;

    [Header("Win Condition")]
    [SerializeField] private int killsRequiredToWin = 3;

    void Start()
    {
        Time.timeScale = 1f;
    }

    private void OnEnable()
    {
        // Subscribe to the score change event
        ScoreManager.OnScoreChanged += CheckWinCondition;
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid memory leaks/errors
        ScoreManager.OnScoreChanged -= CheckWinCondition;
    }

    private void CheckWinCondition(int currentScore)
    {
        // If the score reaches or exceeds the requirement, show win screen
        if (currentScore >= killsRequiredToWin)
        {
            ShowWinScreen();
        }
    }

    public void ShowWinScreen()
    {
        // Safety check to ensure we don't trigger it multiple times
        if (winCanvas.activeSelf) return;

        winCanvas.SetActive(true);
        if (winSound != null) winSound.Play();

        Time.timeScale = 0f; // Pauses the game
    }
}