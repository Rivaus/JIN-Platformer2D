using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class MenuControllerPlayerManager : MonoBehaviour
{

    [SerializeField]
    PlayerController player;

    [SerializeField]
    private  TMP_InputField gravity;

    [SerializeField]
    private  TMP_InputField walkSpeed;
    [SerializeField]
    private  TMP_InputField walkAcceleration;

    [SerializeField]
    private  TMP_InputField runSpeed;
    [SerializeField]
    private  TMP_InputField runAcceleration;

    [SerializeField]
    private  TMP_InputField airSpeed;
    [SerializeField]
    private  TMP_InputField airAcceleration;

   

    private void Start()
    {
        gravity.text = player.gravity.ToString();

        walkSpeed.text = player.walkSpeed.ToString();
        walkAcceleration.text = player.walkAcceleration.ToString();

        runSpeed.text = player.runSpeed.ToString();
        runAcceleration.text = player.runAcceleration.ToString();

        airSpeed.text = player.airSpeed.ToString();
        airAcceleration.text = player.airAcceleration.ToString();
    }

    public void OnGravityChanged(float gravity)
    {
        player.gravity = gravity;
    }
}
