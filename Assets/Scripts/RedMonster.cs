using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMonster : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void killItself()
    {
        Debug.Log("红色怪兽消失");
        this.transform.Translate(0, 20, 0);

    }

    void back()
    {
        Debug.Log("红色怪兽回归");
        this.transform.Translate(0, -20, 0);
    }
}
