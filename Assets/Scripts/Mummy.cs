using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mummy : MonoBehaviour
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
        Debug.Log("木乃伊消失");
        this.transform.Translate(0, 20, 0);

    }

    void back()
    {
        Debug.Log("木乃伊回归");
        this.transform.Translate(0, -20, 0);
    }
}
