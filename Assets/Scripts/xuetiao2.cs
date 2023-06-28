using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class xuetiao2 : MonoBehaviour
{
    public Slider sld;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerbldCtrl(int blood)
    {
        sld.value = (float)blood / 10;      // 人物血量条变化
    }
}
