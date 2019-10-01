using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private float speed = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Jump"))
        {
            Debug.Log("JUMP !!  JUMP !!  JUMP!!");
        }
        else if (Input.GetButton("Dash"))
        {
            Debug.Log("Dashi Dashi !!!");
        }

        Debug.Log(Input.GetAxis("Horizontal") + "  Taille Quentin : " + Input.GetAxis("Vertical"));
    }
}
