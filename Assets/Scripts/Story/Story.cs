using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//这个脚本有bug，暂时不使用

public class Story : MonoBehaviour
{
    private float charsPerSecond = 0.05f;//打字时间间隔
    private string words1;//保存需要显示的文字
    private string words2;//保存需要显示的文字

    private int mytag = 1;
    private bool isActive = false;
    private float timer;//计时器
    public Text myText;
    private int currentPos = 0;//当前打字位置
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        isActive = true;
        charsPerSecond = Mathf.Max(0.05f, charsPerSecond);
        myText.text = "";//获取Text的文本信息，保存到words中，然后动态更新文本显示内容，实现打字机的效果
        words1 = "从前在东方有个奇异的国家——楠春，" +
            "这个国家有着一个与历史上任何一个国家都不同的特点：" +
            "不论男女，从出生下来就是双马尾，拥有双马尾并不是一件很特别的事情，" +
            "但是伴随他们出生的双马尾给了他们与其他人不同的神奇力量，" +
            "使得他们能够感知周围环境的元素，并拥有转化元素的能力。\n" +
            "在西方的永恒之土上沉睡着被称为“源初”的恶魔，" +
            "传说它是上古宇宙之神的第七个子嗣，为了净化这片大陆的源初之恶，" +
            "它吞噬深渊，将自己封印在永恒之土的某个难觅之处。\n"+
            "世事难料，在某一天，这片被神明眷顾的大陆上又出生了灾厄之种，它产生的力量凝聚，" +
            "成为名为“塔耳塔洛斯”的巨大飞龙，它飞往极东之地，寻觅这片土地上的人民拥有的元素力量，" +
            "想要唤醒“源初”恶魔，为世间再度带来灾厄。\n在这片土地上，" +
            "有部分人通过转化元素，能够将元素附于武器上，借此来使用元素元素魔法，" +
            "这部分人被众人称作“魔导士”。随着塔耳塔洛斯的出生，许多地方遭遇了怪物的袭击，" +
            "国家的大部分土地沦陷，在灾厄来临之际，国王召集全国各地的魔导士讨伐恶龙。" +
            "经过半个月的苦战，许多魔导士和战士牺牲，国家处于灭亡的边缘。";

        words2 = "在这个国家的某个地方，有一个女孩，出生的时候与其他人不同" +
            "，她并没有一头双马尾，刚出生的时候，大家称她是“恶魔之子”，" +
            "要将她杀死，以防后患。但因为父母的拼死保护，她最终得以活了下来。" +
            "她的整个童年在深山之中度过，除了父母之外，没有任何人能与她接触。" +
            "女孩的父母给她取名为“生南”，生南就这样度过了十余年，终于在成年这一天，" +
            "她长出了一头双马尾，但这两簇头发却是与众不同的银蓝色，仿佛蕴藏着某种力量。\n" +
            "恶龙侵袭的时候，生南所在的镇被恶龙的吐息侵蚀，她的父母在这场灾难中没能幸免，" +
            "但女孩却幸运的存活了下来，为了给父母报仇 ，她成为魔导士，踏上了讨龙之路。" +
            "成为了魔导士的她天赋异禀，很快就能熟练地使用元素魔法，并且她的魔法总是带着和她发色一样的银蓝色光芒，" +
            "拥有能够让魔物震慑的力量。征战不久的生南被国王召见，国王赐予她象征力量与光明的“天空之剑”，" +
            "这把剑是“源初”吞噬深渊前这片大陆上仅存的光明的凝聚体，它与她的头发浑然一色，" +
            "天蓝色的剑刃上隐隐闪烁着银光。带着这神圣的恩泽，生南再次出发了，这次不仅是为了自己的复仇，更是为了守护与使命……";
    }

    // Update is called once per frame
    void Update()
    {
        OnStartWriter();
    }


    public void StartEffect()
    {
        isActive = true;
    }

    /// 执行打字任务
    public void OnStartWriter()
    {
        if (isActive && mytag==1)
        {
            timer += Time.deltaTime;
            if (timer >= charsPerSecond)
            {//判断计时器时间是否到达
                timer = 0;
                currentPos += 1;
                myText.text = words1.Substring(0, currentPos);//刷新文本显示内容
                //Debug.Log(myText.text);
                //Debug.Log(words.Substring(0, currentPos));
                if (currentPos >= words1.Length)
                {
                    mytag = 2;
                    isActive = false;
                    myText.text = words1;
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && mytag==1)
        {
            Debug.Log(mytag);
            mytag = 2;
            isActive = false;
            myText.text = words1;
        }
        else if (Input.GetMouseButtonDown(0) && (myText.text.Length == words1.Length) && !isActive)
        {
            isActive = true;
            myText.text = words1;
            mytag = 3;
        }

        if (isActive && mytag == 3)
        {
            timer += Time.deltaTime;
            if (timer >= charsPerSecond)
            {//判断计时器时间是否到达
                timer = 0;
                currentPos += 1;
                myText.text = words2.Substring(0, currentPos);//刷新文本显示内容
                //Debug.Log(myText.text);
                //Debug.Log(words.Substring(0, currentPos));
                if (currentPos >= words2.Length)
                {
                    OnFinish();
                    Debug.Log(mytag);
                }
            }
        }
        if (Input.GetMouseButtonDown(0) &&(myText.text.Length < words1.Length)&& mytag == 3)
        {
            Debug.Log(mytag);
            mytag = 4;
            myText.text = words2;
        }

        else if (Input.GetMouseButtonDown(0) && (myText.text.Length == words2.Length) && mytag == 4)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        }

    }
    /// 结束打字，初始化数据
    public void OnFinish()
    {
        Debug.Log(mytag);
        isActive = false;
        timer = 0;
        currentPos = 0;
        myText.text = words2;
    }
}