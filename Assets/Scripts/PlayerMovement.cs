using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions.Must;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 5.0f;              //人物迭代移动速度
    public float endSpeed = 15.0f;          //最终速度
    public float horizontalSpeed = 1.0f;
    float oldspeed = 0f;   // 用于保存当前速度

    int blood = 8;      //人物血量

    int counting = 0;           //跳跃动画计数
    int damageCounting = 0;     //受到伤害动画计数
    int attackCounting = 0;     //攻击动画计数
    int starCounting = 0;       //无敌时间计数
    int redMonsterBackCounting = 0;   //怪兽回归计数
    int MummyBackCounting = 0;        //木乃伊回归计数

    public GameObject redMonster; //红色怪兽对象暂存
    public GameObject Mummy;      //木乃伊对象暂存 

    int pause = 0;              //游戏暂停
    int super = 0;              //角色无敌
    int mapCount = 0;           //boss出场时间为跑完3个巡回

    int Choice = -1;            // 攻击动画选择
    int movementChoice = -1;    // 左右攻击动画选择
    int attacking = 0;          // 判断攻击状态，0为未攻击，1为攻击
    float animatorSpeed = 0.0f; // 动画控制器原速度记录

    float damage = 0.5f;       // 人物对BOSS伤害
    int blueCounting = 0;       // 获得的蓝色地砖数量

    float bossBlood = 100.0f;     //BOSS血量    

   // public ParticleSystem ps;

    int reduce = 0;     // 控制BOSS血量减少

    // int reduce2 = 0;    // 控制人物血量减少/增加

    // int reduce3 = 0;    // 控制人物蓝量减少/增加


    //人物左右移动
    private float Xleft = -1, Xmid = 0, Xright = 1;   // 控制角色左右移动范围
    private bool range_L = false, range_M = true, range_R = false;    //标记角色正在哪个位置
    private float dodgespeed = 5.0f;

    private bool isdodging_toLeft = false;  //获取当前角色移动方向
    private bool isdodging_toRight = false;

    Animator playerAnimator;        //动画控制器

    //更新碰撞盒
    public BoxCollider playerCollider;

    //======== 以下为技能特效属性==============
    public GameObject effect_Explore;
    public GameObject effect_Fire;
    public GameObject effect_Attack;
    //public GameObject theMonster;

    public Text exploreTxt;
    public Text fireTxt;

    private bool isplaying_Fire = false;    //标记变量，控制火焰特效技能冷却：不能短时间连续使用技能
    private bool isplaying_Explore = false;
    private bool isplaying_Attack = false;

    private int timesOfFire = 0;    // 火焰特效释放次数
    private int timesOfExplore = 0;    // 爆炸特效释放次数
    private int timesOfAttack = 0;    // 爆炸特效释放次数

    int i = 0;
    //==================================================

    void Start()
    {
        // 延迟运行
        //System.Threading.Thread.Sleep(2000);
        //Invoke("Update", 2);

        playerAnimator = this.GetComponent<Animator>();
        animatorSpeed = playerAnimator.speed;
        Debug.Log(animatorSpeed);
      //  ps.Stop();
        effect_Explore.GetComponent<ParticleSystem>().Stop();   // 保证粒子特效出场的时候不会播放一次
        effect_Fire.GetComponent<ParticleSystem>().Stop();
        effect_Attack.GetComponent<ParticleSystem>().Stop();
        timesOfFire = 1;    //获得了5次释放火焰特效
        timesOfExplore = 1; //获得了5次爆炸特效释放机会
        exploreTxt.text = timesOfExplore.ToString();
        fireTxt.text = timesOfFire.ToString();
        timesOfAttack = 100; //获得10次特殊攻击
    }

    // Update is called once per frame
    void Update()
    {
        // BOSS血量为0或小于0结束游戏
        if (bossBlood <= 0.0f)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("win");
        }


        //按esc暂停游戏
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause = 1;
        }

        //血量为0
        if (blood == 0)
        {
            Debug.Log("人物死亡");
            //播放死亡动画
            playerAnimator.SetBool("Die", true);
            speed = 0;
            ////暂停游戏
            //pause = 1;
            // 跳转到游戏结束画面
            AnimatorStateInfo animatorStateInfo;
            animatorStateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
            if((animatorStateInfo.normalizedTime>1.0f)&&(animatorStateInfo.IsName("Die")))
                UnityEngine.SceneManagement.SceneManager.LoadScene("over");
        }

        // 游戏暂停
        if(pause == 1)
        {
            // 暂停人物
            if (speed != 0)
            {
                oldspeed = speed;
                speed = 0;
            }

            // 加载暂停场景
            SceneManager.LoadSceneAsync("pause", LoadSceneMode.Additive);
            Time.timeScale = 0;
            pause = 0;
        }


        //游戏未暂停
        if (pause == 0)
        {
            if (oldspeed != 0)
                speed = oldspeed;
            //人物自动前进
            this.gameObject.transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));


            if (range_M && Input.GetKeyDown(KeyCode.A) && !isdodging_toLeft && !isdodging_toRight) //允许闪避的条件，此时角色在中线上，准备向左移动
            {
                isdodging_toLeft = true;
                range_M = false;
                range_L = true;
                //Debug.Log("A\n");
            }
            else if (range_M && Input.GetKeyDown(KeyCode.D) && !isdodging_toLeft && !isdodging_toRight) //允许闪避的条件，此时角色在中线上，准备向右移动
            {
                isdodging_toRight = true;
                range_M = false;
                range_R = true;
                //Debug.Log("D\n");
            }
            else if (range_L && Input.GetKeyDown(KeyCode.D) && !isdodging_toLeft && !isdodging_toRight) //允许闪避的条件，此时角色在中线左边，准备向右移动
            {
                isdodging_toRight = true;
                //Debug.Log("D\n");
            }
            else if (range_R && Input.GetKeyDown(KeyCode.A) && !isdodging_toLeft && !isdodging_toRight) //允许闪避的条件，此时角色在中线右边，准备向左移动
            {
                isdodging_toLeft = true;
               // Debug.Log("A\n");
            }

            // 人物跳跃
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (this.transform.position.y == 0)
                {
                    playerAnimator.SetBool("isJump", true);
                    //碰撞盒跳跃
                    playerCollider.center = new Vector3(playerCollider.center.x, 1.5f, playerCollider.center.z);
                    // 跳跃音效
                    gameObject.BroadcastMessage("randomPlay",1);
                    counting = 1;
                }
            }

            

            // 按S落下或攻击
            if (Input.GetKeyDown(KeyCode.S))
            {
                // 如果在地面上
                if (playerCollider.center.y < 1.0f)
                {
                    Debug.Log("在地面，启动攻击");
                    //随机选择多种攻击模式
                    //随机值
                    Choice = Random.Range(0, 3);

                    //开始攻击
                    attacking = 1;

                    // 第一种动画
                    if (Choice == 0)
                    {
                        playerAnimator.SetBool("isAttack", true);
                    }
                    // 第二种动画
                    else if (Choice == 1)
                    {
                        playerAnimator.SetBool("isAttack2", true);
                    }
                    //第三种动画
                    else if (Choice == 2)
                    {
                        playerAnimator.SetBool("isAttack3", true);
                    }

                    // 攻击音效
                    gameObject.BroadcastMessage("randomPlay", 3);

                    attackCounting = 1;
                }

                // 如果在天上
                if (playerCollider.center.y > 1.0f)
                {
                    Debug.Log("在天上，赶快落地");
                    playerAnimator.speed = 20.0f;
                    playerCollider.center = new Vector3(playerCollider.center.x, 0.6f, playerCollider.center.z);
                    counting = 59;
                }
            }


            //左右移动执行部分
            if (isdodging_toLeft && range_L)    //角色在中线左边，向左移动
            {
                this.gameObject.transform.Translate(new Vector3(-dodgespeed * Time.deltaTime, 0, 0));
                //开始攻击动画
                if (movementChoice == -1)
                {
                    movementChoice = Random.Range(0, 2);
                    //开始攻击

                    //在这里造成伤害——————————————————————————————————————

                    // 攻击音效
                    gameObject.BroadcastMessage("randomPlay", 3);

                    attacking = 1;
                }
                if (movementChoice == 0)
                {
                    playerAnimator.SetBool("isAttack", true);
                   // Debug.Log("开始isAttack");
                }
                else if (movementChoice == 1)
                {
                    playerAnimator.SetBool("isAttack2", true);
                  //  Debug.Log("开始isAttack2");
                }


                if (this.transform.position.x <= Xleft)
                {
                    //结束攻击动画
                    if (movementChoice == 0)
                    {
                        playerAnimator.SetBool("isAttack", false);
                      //  Debug.Log("结束isAttack");
                    }
                    else if (movementChoice == 1)
                    {
                        playerAnimator.SetBool("isAttack2", false);
                        Debug.Log("结束isAttack2");
                    }
                    //结束攻击
                    attacking = 0;
                    movementChoice = -1;
                  //  Debug.Log("设置moveChoice为-1");

                    //恢复伤害初值
                    damage = 10;

                    isdodging_toLeft = false;
                }
            }
            else if (isdodging_toRight && range_L)     //角色在中线左边，向右移动
            {
                this.gameObject.transform.Translate(new Vector3(dodgespeed * Time.deltaTime, 0, 0));

                //开始攻击动画
                if (movementChoice == -1)
                {
                    movementChoice = Random.Range(0, 2);
                    //开始攻击

                    // 攻击音效
                    gameObject.BroadcastMessage("randomPlay", 3);

                    //在这里造成伤害——————————————————————————————————————

                    attacking = 1;
                }
                if (movementChoice == 0)
                {
                    playerAnimator.SetBool("isAttack", true);
                  //  Debug.Log("开始isAttack");
                }
                else if (movementChoice == 1)
                {
                    playerAnimator.SetBool("isAttack2", true);
                   // Debug.Log("开始isAttack2");
                }

                if (this.transform.position.x >= Xmid)
                {
                    //结束攻击动画
                    if (movementChoice == 0)
                    {
                        playerAnimator.SetBool("isAttack", false);
                       // Debug.Log("结束isAttack");
                    }
                    else if (movementChoice == 1)
                    {
                        playerAnimator.SetBool("isAttack2", false);
                      //  Debug.Log("结束isAttack2");
                    }
                    //结束攻击
                    attacking = 0;
                    movementChoice = -1;
                   // Debug.Log("设置moveChoice为-1");

                    //恢复伤害初值
                    damage = 10;

                    isdodging_toRight = false;
                    range_L = false;
                    range_M = true;
                }
            }

            if (isdodging_toRight && range_R)    //角色在中线右边，向右移动
            {
                this.gameObject.transform.Translate(new Vector3(dodgespeed * Time.deltaTime, 0, 0));

                //开始攻击动画
                if (movementChoice == -1)
                {
                    movementChoice = Random.Range(0, 2);
                    //开始攻击

                    // 攻击音效
                    gameObject.BroadcastMessage("randomPlay", 3);

                    //在这里造成伤害——————————————————————————————————————

                    attacking = 1;
                }
                if (movementChoice == 0)
                {
                    playerAnimator.SetBool("isAttack", true);
                  //  Debug.Log("开始isAttack");
                }
                else if (movementChoice == 1)
                {
                    playerAnimator.SetBool("isAttack2", true);
                   // Debug.Log("开始isAttack2");
                }

                if (this.transform.position.x >= Xright)
                {
                    //结束攻击动画
                    if (movementChoice == 0)
                    {
                        playerAnimator.SetBool("isAttack", false);
                     //   Debug.Log("结束isAttack");
                    }
                    else if (movementChoice == 1)
                    {
                        playerAnimator.SetBool("isAttack2", false);
                      //  Debug.Log("结束isAttack2");
                    }
                    //结束攻击
                    attacking = 0;
                    movementChoice = -1;
                  //  Debug.Log("设置moveChoice为-1");

                    //恢复伤害初值
                    damage = 10;

                    isdodging_toRight = false;
                }
            }

            if (isdodging_toLeft && range_R)    //角色在中线右边，向左移动
            {
                this.gameObject.transform.Translate(new Vector3(-dodgespeed * Time.deltaTime, 0, 0));

                //开始攻击动画
                if (movementChoice == -1)
                {
                    movementChoice = Random.Range(0, 2);
                    //开始攻击

                    // 攻击音效
                    gameObject.BroadcastMessage("randomPlay", 3);

                    //在这里造成伤害——————————————————————————————————————

                    attacking = 1;
                }
                if (movementChoice == 0)
                {
                    playerAnimator.SetBool("isAttack", true);
                  //  Debug.Log("开始isAttack");
                }
                else if (movementChoice == 1)
                {
                    playerAnimator.SetBool("isAttack2", true);
                  //  Debug.Log("开始isAttack2");
                }

                if (this.transform.position.x <= Xmid)
                {
                    //结束攻击动画
                    if (movementChoice == 0)
                    {
                        playerAnimator.SetBool("isAttack", false);
                    //    Debug.Log("结束isAttack");
                    }
                    else if (movementChoice == 1)
                    {
                        playerAnimator.SetBool("isAttack2", false);
                     //   Debug.Log("结束isAttack2");
                    }
                    //结束攻击
                    attacking = 0;
                    movementChoice = -1;
                   // Debug.Log("设置moveChoice为-1");

                    //恢复伤害初值
                    damage = 10;


                    isdodging_toLeft = false;
                    range_R = false;
                    range_M = true;
                }
            }


            //全部的计数部分

            //跳跃计数
            if (counting >= 1)
            {
                counting += 1;
            }
            //受伤计数
            if (damageCounting >= 1)
            {
                damageCounting += 1;
            }
            //攻击计数
            if (attackCounting >= 1)
            {
                attackCounting += 1;
            }
            //无敌加速时间
            if (starCounting >= 1)
            {
                starCounting += 1;
            }
            //怪兽回归
            if (redMonsterBackCounting >= 1)
            {
                redMonsterBackCounting += 1;
            }
            //木乃伊回归
            if (MummyBackCounting >= 1)
            {
                MummyBackCounting += 1;
            }

            //————————————————————————————————————————————————————————————————

            //跳跃计数截止
            if (counting >= 60)
            {
                if (playerAnimator.speed != animatorSpeed)
                {
                  //  Debug.Log("恢复播放速度");
                    playerAnimator.speed = animatorSpeed;
                }
                playerAnimator.SetBool("isJump", false);
                playerCollider.center = new Vector3(playerCollider.center.x, 0.6f, playerCollider.center.z);
                counting = 0;
            }
            //伤害计数截止
            if (damageCounting >= 50)
            {
                playerAnimator.SetBool("isHit", false);
                damageCounting = 0;
            }
            //攻击计数截止
            if (attackCounting >= 30)
            {
                if (Choice == 0)
                {
                    playerAnimator.SetBool("isAttack", false);
                }
                else if (Choice == 1)
                {
                    playerAnimator.SetBool("isAttack2", false);
                }
                else if (Choice == 2)
                {
                    playerAnimator.SetBool("isAttack3", false);
                }
                //结束攻击
                attacking = 0;
                attackCounting = 0;

                //恢复伤害初值
                damage = 10;
            }
            //无敌加速时间截止
            if (starCounting >= 500)
            {
                starCounting = 0;
                speed = 5.0f;
                super = 0;
            }
            //怪兽回归截止
            if (redMonsterBackCounting >= 50)
            {
                redMonsterBackCounting = 0;
                redMonster.SendMessage("back");
            }
            //木乃伊回归截止
            if (MummyBackCounting >= 50)
            {
                MummyBackCounting = 0;
                Mummy.SendMessage("back");
            }

            // 移动速度的迭代
            if (speed <= endSpeed && super == 0)
            {
                speed += 0.5f * Time.deltaTime;
            }


            //boss出场检测
            if (mapCount > 2)
            {
              //  Debug.Log("boss出场");
            }
        }

        isplaying_Fire = effect_Fire.GetComponent<ParticleSystem>().isPlaying;
        isplaying_Explore = effect_Explore.GetComponent<ParticleSystem>().isPlaying;
        isplaying_Attack = effect_Attack.GetComponent<ParticleSystem>().isPlaying;
        if (Input.GetKeyUp(KeyCode.L) && !isplaying_Explore && timesOfExplore > 0) //按L爆炸
        {
            play_effectExplore();
            isplaying_Explore = true;
        }
        if (Input.GetKeyUp(KeyCode.K) && !isplaying_Fire && timesOfFire > 0)   //按K燃烧
        {
            play_effeftFire();
            isplaying_Fire = true;
        }
        if (Input.GetKeyUp(KeyCode.J) && !isplaying_Attack && timesOfAttack > 0)   //按J特殊攻击
        {
            play_effectAttack();
            isplaying_Attack = true;
        }
        
    }//end Update()

    //=====技能特效=======
    public void play_effectExplore()
    {
        // 爆炸特效
        effect_Explore.GetComponent<ParticleSystem>().Play();   //播放
        gameObject.BroadcastMessage("randomPlay", 7);//技能音效：爆炸
        bossBlood -= 5; //boss扣血
        timesOfExplore--;   //用掉一次
        exploreTxt.text = timesOfExplore.ToString();

        reduce += 10;
        gameObject.BroadcastMessage("BossbldCtrl", reduce);
        Debug.Log(timesOfExplore);
    }
    public void play_effeftFire()
    {
        //燃烧特效
        effect_Fire.GetComponent<ParticleSystem>().Play();
        gameObject.BroadcastMessage("randomPlay", 5);//技能音效：火焰
        bossBlood -= 4; //boss扣血
        timesOfFire--;
        fireTxt.text = timesOfFire.ToString();

        reduce += 8;
        gameObject.BroadcastMessage("BossbldCtrl", reduce); 
        
    }

    public void play_effectAttack()
    {
        //特殊攻击特效
        effect_Attack.GetComponent<ParticleSystem>().Play();
        timesOfAttack--;
        //随机选择多种攻击模式
        //随机值
        Choice = Random.Range(0, 3);

        //开始攻击
        attacking = 1;
        // 第一种动画
        if (Choice == 0)
        {
            playerAnimator.SetBool("isAttack", true);
        }
        // 第二种动画
        else if (Choice == 1)
        {
            playerAnimator.SetBool("isAttack2", true);
        }
        //第三种动画
        else if (Choice == 2)
        {
            playerAnimator.SetBool("isAttack3", true);
        }

        // 攻击BOSS音效
        gameObject.BroadcastMessage("randomPlay", 2);

        attackCounting = 1;

        // 造成伤害
        //    ps.Play();      // 攻击特效

        reduce += 2;    // 记录攻击次数

        gameObject.BroadcastMessage("BossbldCtrl", reduce);       // BOSS血量每次扣除0.5
        bossBlood = bossBlood - 1.0f;
    }
    //===============
    //碰撞检测
    void OnTriggerEnter(Collider collider)
    {
        //检测到终点
        if (collider.gameObject.tag == "WayEnd")
        {
            this.transform.position = new Vector3(0, 0, 0);
            mapCount += 1;
        }

        //检测到尖刺
        if (collider.gameObject.tag == "Spikes" && super == 0)
        {
            damageCounting = 1;
            Debug.Log("受到伤害，扣除一滴血");
            playerAnimator.SetBool("isHit", true);      // 播放受伤动画
            blood -= 1;
            gameObject.BroadcastMessage("PlayerbldCtrl", blood);    // 人物血条减少
        }

        //检测到小怪
        if(collider.gameObject.tag == "Monster" && super == 0)
        {
            if(attacking == 1)
            {
                //小怪移走
                Debug.Log("消灭红色怪兽");
                collider.gameObject.SendMessage("killItself");
                //开始回归计数
                redMonsterBackCounting = 1;
                redMonster = collider.gameObject;
                
            }
            else if(attacking == 0)
            {
                Debug.Log("受到伤害，扣除四滴血");
                damageCounting = 1;
                playerAnimator.SetBool("isHit", true);
                blood -= 4;
                gameObject.BroadcastMessage("PlayerbldCtrl", blood);
            }
        }

        //落水死亡
        if(collider.gameObject.tag == "Water" && super == 0)
        {
            Debug.Log("落水");
            blood = 0;
            // 死亡音效
            gameObject.BroadcastMessage("randomPlay", 4);
        }

        //蓝色地板砖加蓝量，加一次爆炸技能
        if(collider.gameObject.tag == "BlueButton" && super == 0)
        {
            timesOfExplore++;
            exploreTxt.text = timesOfExplore.ToString();
            blueCounting += 1;
            gameObject.BroadcastMessage("BlueCtrl", blueCounting);
            // 在这里显示已经存储的蓝色地砖数

            Debug.Log("获得蓝色地砖数" + blueCounting);

            //如果获得三个蓝色地砖
            if (blueCounting == 3)
            {
                // 角色伤害为10，限定一次攻击
                //damage = 10.0f;
                reduce += 20;
                gameObject.BroadcastMessage("BossbldCtrl", reduce);
                bossBlood = bossBlood - 10.0f;

                // 特效
                play_effectExplore();
                isplaying_Explore = true;

                // 技能音效
                gameObject.BroadcastMessage("randomPlay", 5);

                blueCounting = 0;
                gameObject.BroadcastMessage("BlueCtrl", blueCounting);
            }
            
        }

        //红色地板砖加血,加一次火焰技能
        if(collider.gameObject.tag == "RedButton")
        {
            timesOfFire++;
            fireTxt.text = timesOfFire.ToString();
            if (blood <= 9&&blood>=1)
            {
                blood += 1;
                gameObject.BroadcastMessage("PlayerbldCtrl", blood);
                Debug.Log("血量+1");
            }
        }

        //木乃伊扣除两滴血，减速
        if(collider.gameObject.tag == "Mummy" && super == 0)
        {
            if(attacking == 1)
            {
                Debug.Log("消灭木乃伊");
                collider.gameObject.SendMessage("killItself");
                //开始回归计数
                MummyBackCounting = 1;
                Mummy = collider.gameObject;
            }
            else if(attacking == 0)
            {
                speed /= 2;
                Debug.Log("收到伤害，扣除两滴血");
                blood = blood - 2;
                gameObject.BroadcastMessage("PlayerbldCtrl", blood);
                damageCounting = 1;
                playerAnimator.SetBool("isHit", true);
            }
        }

        //星星无敌加速
        if(collider.gameObject.tag == "Star")
        {
            Debug.Log("星星无敌加速");
            speed = 25;
            starCounting = 1;
            super = 1;

            // 星星音效
            gameObject.BroadcastMessage("randomPlay", 6);
        }

        //火球碰撞
        if (collider.gameObject.tag == "Fireball")
        {
            Debug.Log("收到火球攻击");
            blood = blood - 1;
            gameObject.BroadcastMessage("PlayerbldCtrl", blood);
        }

    }
}


