using UnityEngine;
using UnityEngine.UI;
public class coinscore : MonoBehaviour
{
    public static int scorevalue = 0;
    Text scoretext;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoretext=GetComponent<Text>();
        scoretext.text = "Score:" + scorevalue;
    }
}
