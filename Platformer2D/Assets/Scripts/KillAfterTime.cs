using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAfterTime : MonoBehaviour
{
    [SerializeField]
    float killTime;

    private void Start()
    {
        Destroy(gameObject, killTime);
    }
}
