using UnityEngine;
using System.Collections;
public class calmController : MonoBehaviour
{
    public GameObject clamClosed;
    public GameObject clamOpen;
    public GameObject pearl;
    public float openTime = 2f;
    public float closeTime = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(OpenCloseClam());
    }

    IEnumerator OpenCloseClam()
    {
        while (true)
        {
            // فتح
            clamClosed.SetActive(false);
            clamOpen.SetActive(true);
            pearl.SetActive(true);

            yield return new WaitForSeconds(openTime);

            // قفل
            clamOpen.SetActive(false);
            clamClosed.SetActive(true);
            pearl.SetActive(false);

            yield return new WaitForSeconds(closeTime);
        }
    }
}

