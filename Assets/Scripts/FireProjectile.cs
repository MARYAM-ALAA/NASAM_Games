using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    [SerializeField] private int scorePerKill = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Fire hit: " + other.name); // عشان نتأكد إنه شاف التصادم

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy!"); // نشوفها في الـ Console

            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(scorePerKill);
            }

            // لو العدو جواه Child عليه الكوليدر
            EnemyController enemy = other.GetComponentInParent<EnemyController>();

            if (enemy != null)
            {
                Destroy(enemy.gameObject);
            }
            else
            {
                Destroy(other.gameObject);
            }

            Destroy(gameObject); // تدمير النار
        }
    }
}
