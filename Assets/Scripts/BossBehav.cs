using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehav : MonoBehaviour
{

    public float FireballSpeed  = 1f;
    int BossHP = 100;

    public GameObject ATKstartPos1;
    public GameObject ATKstartPos2;
    public GameObject ATKstartPos3;

    public ParticleSystem fireball;

    Animator Boss_ani;

    int tscount;
    // Start is called before the first frame update
    void Start()
    {
        Boss_ani = gameObject.GetComponent<Animator>();
        fireball.Stop();
       
    }

    // Update is called once per frame
    void Update()
    {
        fireball.transform.Translate(Vector3.up * Time.deltaTime * FireballSpeed);

        if (tscount > 300 && BossHP != 0)
        {
            FireBallATK();
            tscount = 0;
        }
        else tscount++;

        if(BossHP == 0)
        {
            BossDie();
        }
    }

    void FireballDisappear()
    {
        fireball.Stop() ;
    }

    void FireBallATK()
    {

        int flag = Random.Range(1, 4);
        switch(flag)
        {
            case 1:
                Debug.Log("Left Atk");
                BossATK();
                Invoke("FireballDisappear", 0.6f);
                Invoke("FBRepos1",1.5f);
                fireball.Play();
                break;
            case 2:
                Debug.Log("Mid Atk");
                BossATK();
                Invoke("FireballDisappear", 0.6f);
                Invoke("FBRepos2",1.5f);
                fireball.Play();
                break;
            case 3:
                Debug.Log("Right Atk");
                BossATK();
                Invoke("FireballDisappear", 0.6f);
                Invoke("FBRepos3",1.5f);
                fireball.Play();
                break;
            default:break;
        }



    }

    void FBRepos1()
    {
        fireball.transform.position = ATKstartPos1.transform.position;
    }

    void FBRepos2()
    {
        fireball.transform.position = ATKstartPos2.transform.position;
    }

    void FBRepos3()
    {
        fireball.transform.position = ATKstartPos3.transform.position;
    }

    void BossATK()
    {
        Boss_ani.SetBool("Attack",true);
        Boss_ani.SetBool("Walk", false);
        Boss_ani.SetBool("isHit", false);
        Boss_ani.SetBool("Die",false);
        Invoke("BossWalk", 1.0f);
    }

    void BossWalk()
    {
        Boss_ani.SetBool("Attack", false);
        Boss_ani.SetBool("Walk", true);
        Boss_ani.SetBool("isHit", false);
        Boss_ani.SetBool("Die", false);
        Invoke("BossWalk", 1.0f);
    }

    void BossIsHit()
    {
        Boss_ani.SetBool("Attack", true);
        Boss_ani.SetBool("Walk", false);
        Boss_ani.SetBool("isHit", true);
        Boss_ani.SetBool("Die", false);
        Invoke("BossWalk", 1.0f);
    }

    void BossDie()
    {
        Boss_ani.SetBool("Attack", false);
        Boss_ani.SetBool("Walk", false);
        Boss_ani.SetBool("isHit", false);
        Boss_ani.SetBool("Die", true);
        Invoke("BossWalk", 1.0f);
    }

}
