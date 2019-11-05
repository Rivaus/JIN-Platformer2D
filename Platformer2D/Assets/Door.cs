using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField]
    string levelName;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Pomme");
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(levelName, LoadSceneMode.Single);
        }
    }
}
