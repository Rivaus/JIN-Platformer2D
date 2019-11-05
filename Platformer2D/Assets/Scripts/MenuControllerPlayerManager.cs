using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class MenuControllerPlayerManager : MonoBehaviour
{
    [SerializeField]
    GameObject UImodif;

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

    private void Awake()
    {
        gravity.onValueChanged.AddListener((value) => SetGravity(float.Parse(value)));

        walkSpeed.onValueChanged.AddListener((value) => SetWalkSpeed(float.Parse(value)));
        walkAcceleration.onValueChanged.AddListener((value) => SetWalkAcceleration(float.Parse(value)));

        runSpeed.onValueChanged.AddListener((value) => SetRunSpeed(float.Parse(value)));
        runAcceleration.onValueChanged.AddListener((value) => SetRunAcceleration(float.Parse(value)));

        airSpeed.onValueChanged.AddListener((value) => SetAirSpeed(float.Parse(value)));
        airAcceleration.onValueChanged.AddListener((value) => SetAirAcceleration(float.Parse(value)));
    }

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

    private void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            UImodif.SetActive(!UImodif.active);
        }
    }

    public void Reset()
    {
        player.gravity = 65;
        player.runSpeed = 20;
        player.runAcceleration = 60;

        player.walkSpeed = 10;
        player.walkAcceleration = 50;

        player.airSpeed = 5;
        player.airAcceleration = 30;

        gravity.text = "65";

        walkSpeed.text = "10";
        walkAcceleration.text = "50";

        runSpeed.text = "20";
        runAcceleration.text = "60";

        airSpeed.text = "5";
        airAcceleration.text = "30";
    }

    public void SetGravity(float gravity)
    {
        player.gravity = gravity;
    }

    public void SetWalkSpeed(float speed)
    {
        player.walkSpeed = speed;
    }

    public void SetWalkAcceleration(float acc)
    {
        player.walkAcceleration = acc;
    }

    public void SetAirSpeed(float speed)
    {
        player.airSpeed = speed;
    }

    public void SetAirAcceleration(float acc)
    {
        player.airAcceleration = acc;
    }

    public void SetRunSpeed(float speed)
    {
        player.walkSpeed = speed;
    }

    public void SetRunAcceleration(float acc)
    {
        player.walkAcceleration = acc;
    }
}
