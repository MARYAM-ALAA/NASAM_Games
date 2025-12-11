using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform dragon; // التنين في النص

    private void Awake()
    {
        if (dragon == null)
        {
            GameObject dragonGO = GameObject.FindGameObjectWithTag("Player");
            if (dragonGO != null)
                dragon = dragonGO.transform;
        }
    }

    // 👈 ده اللي هيناديه المانجر
    public EnemyController SpawnOne()
    {
        if (enemyPrefab == null || dragon == null)
        {
            Debug.LogWarning("EnemySpawner: Missing prefab or dragon reference");
            return null;
        }

        GameObject enemyInstance = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        EnemyController enemyController = enemyInstance.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            int dir = (dragon.position.x > transform.position.x) ? 1 : -1;
            enemyController.Init(dir);
        }

        return enemyController;
    }
}
