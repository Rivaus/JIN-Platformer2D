using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    BoxCollider boxCollider;
    Vector2 velocity;

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
        
    }

    void ComputeMovment()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        velocity.x = Mathf.MoveTowards(velocity.x, walkSpeed * moveInput, walkAcceleration * Time.deltaTime);

        transform.Translate(velocity * Time.deltaTime);
    }

}
