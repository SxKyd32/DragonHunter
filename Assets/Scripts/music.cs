using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class music: MonoBehaviour
{
    public Slider sld;
    public Text txt;
   // public AudioSource BGMSource;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
            
        

    }
    public void sldFun()
    {
        txt.text = sld.value.ToString();//handle子物体 
       // BGMSource.volume = sld.value;
    }
 


}