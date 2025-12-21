//using UnityEngine;

//public class HoleApple : MonoBehaviour
//{
//    public string targetName = "holetarget"; // اسم التفاحة
//    public AudioSource audioSource;

//    void Start()
//    {
//        audioSource = GetComponent<AudioSource>();
//    }

//    void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Player") && gameObject.name == targetName)
//        {
//            Debug.Log("Player collided with the apple!");
//            if (audioSource != null && !audioSource.isPlaying)
//            {
//                audioSource.Play();
//            }
//        }
//    }
//}
