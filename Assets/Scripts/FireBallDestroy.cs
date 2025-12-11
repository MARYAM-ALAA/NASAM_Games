using UnityEngine;

public class FireAutoDestroy : MonoBehaviour
{
    [SerializeField] private float lifeTime = 0.7f; // ظبّطيها على قد طول الأنيميشن

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
