using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject camera1;
    public GameObject camera2;
    // Start is called before the first frame update
    void Start()
    {
        camera2.GetComponent<Camera>().enabled = false;
        camera2.GetComponent<AudioListener>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        camera1_control();
    }

    private void camera1_control()
    {
        camera1.transform.LookAt(this.transform);   // this是角色
        if (Vector3.Distance(camera1.transform.position, this.transform.position) >= 0.7) //camera1离角色距离
        {
            camera1.transform.Translate(Vector3.forward * 2 * Time.deltaTime, camera1.transform);  //camera1向自身前方移动
            camera1.transform.RotateAround(this.transform.position, Vector3.up, -50 * Time.deltaTime); //camera1同时围绕角色旋转
            camera1.transform.RotateAround(this.transform.position, Vector3.up, -50 * Time.deltaTime); //camera1同时围绕角色旋转
        }
        else
        {
            camera1.GetComponent<Camera>().enabled = false;    //关闭摄像机视角
            camera1.GetComponent<AudioListener>().enabled = false; //声音监听器也得切换

            camera2.GetComponent<Camera>().enabled = true;
            camera2.GetComponent<AudioListener>().enabled = true;
        }
    }
}
