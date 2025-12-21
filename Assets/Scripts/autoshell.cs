using UnityEngine;
using System.Collections;

public class AutoShell : MonoBehaviour
{
    public GameObject closedShell;
    public GameObject openShell;
    public GameObject pearl;

    public float openTime = 2f;
    public float closeTime = 2f;

    private bool isOpen = false;

    void Start()
    {
        closedShell.SetActive(true);
        openShell.SetActive(false);
        pearl.SetActive(false);

        StartCoroutine(ShellRoutine());
    }

    IEnumerator ShellRoutine()
    {
        while (true)
        {
            // فتح المحارة
            closedShell.SetActive(false);
            openShell.SetActive(true);
            pearl.SetActive(true);
            isOpen = true;

            yield return new WaitForSeconds(openTime);

            // قفل المحارة
            closedShell.SetActive(true);
            openShell.SetActive(false);
            pearl.SetActive(false);
            isOpen = false;

            yield return new WaitForSeconds(closeTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger detected: " + collision.name); // هذا لتأكيد التصادم
        if (collision.CompareTag("Player") && isOpen)
        {
            pearl.SetActive(false); // اللؤلؤة تختفي
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "target")
        {
            Destroy(collision.gameObject);

        }
    }
}
