using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingObj : MonoBehaviour
{
    private Rigidbody2D reg;
    private Animator anim;
    public string isrun;

    // Start is called before the first frame update
    void Start()
    {
        reg = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        int x = 0;
        int y = 0;
        
        if (Input.GetKey("right"))
        {
            x = 5 ;
            transform.localScale = new Vector3(4, 7, 1);
            anim.SetBool(isrun, true);
        }
        else if (Input.GetKey("left"))
        {
            x = -5;
            transform.localScale = new Vector3(-4, 7, 1);
            anim.SetBool(isrun, true);
        }
        else if (Input.GetKey("up"))
        {
            y = 5;
            anim.SetBool(isrun, false);
        }
        else if (Input.GetKey("down"))
        {
            y = -5;
            anim.SetBool(isrun, false);
        }
        reg.AddForce(new Vector2(x, y));
    }
}
