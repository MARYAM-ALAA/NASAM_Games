using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int scorevalue = 0;
    Text scoretext;

    void Start()
    {
        scoretext = GetComponent<Text>();
        scoretext.text = "Score 0";
    }

    void Update()
    {
        scoretext.text = "Score " + scorevalue;
    }
}
