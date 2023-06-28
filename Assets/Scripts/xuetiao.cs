using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class xuetiao: MonoBehaviour
{
    public Slider sld;
    public int life;
    private int max;


    // Start is called before the first frame update
    void Start()
    {
        //max = 100;
        float life = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    public void BossbldCtrl(int reduce)
    {
        sld.value = ((float)life - reduce * 0.5f) / 100;       // BOSS血量条变化（减少）
    }

}
