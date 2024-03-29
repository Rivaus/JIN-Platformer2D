﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            ChangeScene("GameScene");
        }
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
