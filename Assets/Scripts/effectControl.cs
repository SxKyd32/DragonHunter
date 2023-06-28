using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//已经全部放在了PlayerMovement.cs中，这脚本没啥用了
public class effectControl : MonoBehaviour
{
    public GameObject effect_Explore;
    public GameObject effect_Fire;
    public GameObject effect_Attack;
    //public GameObject theMonster;

    private bool isplaying_Fire = false;    //标记变量，控制火焰特效技能冷却：不能短时间连续使用技能
    private bool isplaying_Explore = false;
    private bool isplaying_Attack = false;

    private int timesOfFire = 0;    // 火焰特效释放次数
    private int timesOfExplore = 0;    // 爆炸特效释放次数
    private int timesOfAttack = 0;    // 爆炸特效释放次数
    // Start is called before the first frame update
    void Start()
    {
        effect_Explore.GetComponent<ParticleSystem>().Stop();   // 保证粒子特效出场的时候不会播放一次
        effect_Fire.GetComponent<ParticleSystem>().Stop();
        effect_Attack.GetComponent<ParticleSystem>().Stop();
        timesOfFire = 5;    //获得了5次释放火焰特效
        timesOfExplore = 5; //获得了5次爆炸特效释放机会
        timesOfAttack = 10; //获得10次特殊攻击
    }

    // Update is called once per frame
    void Update()
    {
        isplaying_Fire = effect_Fire.GetComponent<ParticleSystem>().isPlaying;
        isplaying_Explore = effect_Explore.GetComponent<ParticleSystem>().isPlaying;
        isplaying_Attack = effect_Attack.GetComponent<ParticleSystem>().isPlaying;
        if (Input.GetKeyUp(KeyCode.L) && !isplaying_Explore && timesOfExplore > 0) //按L爆炸
        {
            play_effectExplore();
            isplaying_Explore = true;
            //Debug.Log("E\n");
          //  Debug.Log(isplaying_Explore);
        }
        if (Input.GetKeyUp(KeyCode.K) && !isplaying_Fire && timesOfFire > 0)   //按K燃烧
        {
            play_effeftFire();
            isplaying_Fire = true;
           // Debug.Log("F\n");
           // Debug.Log(isplaying_Fire);
        }
        if (Input.GetKeyUp(KeyCode.J) && !isplaying_Attack && timesOfAttack > 0)   //按J特殊攻击
        {
            play_effectAttack();
            isplaying_Attack = true;
           // Debug.Log("J\n");
           // Debug.Log(timesOfAttack);
        }
    }
    public void play_effectExplore()
    {
        // 爆炸特效
        effect_Explore.GetComponent<ParticleSystem>().Play();   //播放

        timesOfExplore--;   //用掉一次
    }
    public void play_effeftFire()
    {
        //燃烧特效
        effect_Fire.GetComponent<ParticleSystem>().Play();
        timesOfFire--;
    }

    public void play_effectAttack()
    {
        //特殊攻击特效
        effect_Attack.GetComponent<ParticleSystem>().Play();
        timesOfAttack--;
    }

}
