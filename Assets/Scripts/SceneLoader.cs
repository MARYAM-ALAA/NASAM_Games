using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public AudioSource cheersAudio; // اسحب هنا الـ AudioSource اللي بيشغل cheers

    // الميثود اللي هتشتغل لما نضغط الزرار
    public void LoadFirstScene()
    {
        // وقف الصوت لو موجود
        if (cheersAudio != null && cheersAudio.isPlaying)
        {
            cheersAudio.Stop();
        }

        // افتح المشهد الأول
        SceneManager.LoadScene("Balloon"); // حط هنا اسم الـ Scene الأول
    }
}
