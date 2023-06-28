using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lan : MonoBehaviour
{
    public Slider sld;
    public int life;

    // Start is called before the first frame update
    void Start()
    {
        life = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BlueCtrl(int blue)
    {
        sld.value = (float)blue / 3;    // 人物蓝量变化
    }
}
