using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip1;
    public AudioClip clip2;
    public AudioClip clip3;
    public AudioClip clip4;
    public AudioClip clip5;
    public AudioClip clip6;
    public AudioClip clip7;

    public float Volume;
    public float randomNum;
    public int state;

    // Start is called before the first frame update
    void Start()
    {
        Volume = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void randomPlay(int playState)
    {
        //state = playState;
        //randomNum = Random.Range(1.0f, 4.0f);
        // 跳跃音效
        if(playState == 1)
        {
            audioSource.clip = clip1;
            audioSource.Play();
        }
        // 攻击BOSS音效
        if (playState == 2)
        {
            audioSource.clip = clip2;
            audioSource.Play();
        }
        // 攻击音效
        if (playState == 3)
        {
            audioSource.clip = clip3;
            audioSource.Play();
        }
        // 死亡音效
        if (playState == 4)
        {
            audioSource.clip = clip4;
            audioSource.Play();
        }
        // 技能音效
        if (playState == 5)
        {
            audioSource.clip = clip5;
            audioSource.Play();
        }
        // 星星音效
        if (playState == 6)
        {
            audioSource.clip = clip6;
            audioSource.Play();
        }
        // 爆炸音效
        if (playState == 7)
        {
            audioSource.clip = clip7;
            audioSource.Play();
        }
    }
}
