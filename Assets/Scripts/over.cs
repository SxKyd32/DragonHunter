using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class over : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(onClick);

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void onClick()

    {

        UnityEngine.SceneManagement.SceneManager.LoadScene("over");
        Time.timeScale = 1;
    }


}
