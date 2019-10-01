using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    BoxCollider boxCollider;
    Vector2 velocity;

    private float speed;
    private float acceleration;
    private bool isGrounded = true;

    [SerializeField]
    float gravity;
    [SerializeField]
    float walkSpeed;
    [SerializeField]
    float walkAcceleration;

    [SerializeField]
    float runSpeed;
    [SerializeField]
    float runAcceleration;

    [SerializeField]
    float jumpForce;



    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        Debug.Assert(boxCollider != null, "Le joueur doit posséder une box de collision");
    }

    private void Update()
    {
        ComputeMovment();
        transform.Translate(velocity * Time.deltaTime);
    }

    void ComputeMovment()
    {
        if(Input.GetAxisRaw("Run") > 0)
        {
            speed = runSpeed;
            acceleration = runAcceleration;
        }
        else
        {
            speed = walkSpeed;
            acceleration = walkAcceleration;
        }
        
        float moveInput = Input.GetAxisRaw("Horizontal");
        velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInput, acceleration * Time.deltaTime);

        if (isGrounded && Input.GetButtonDown("Jump") ){
            Jump();
        }
        velocity.y += -gravity*Time.deltaTime;
    }

    //precondition le joueur a appuyé sur Jump et isGrounded
    void Jump()
    {
        velocity.y = jumpForce;
    }
}

