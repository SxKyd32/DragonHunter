using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class goon : MonoBehaviour
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
        SceneManager.UnloadSceneAsync("pause");
        Time.timeScale = 1;
    }


}

