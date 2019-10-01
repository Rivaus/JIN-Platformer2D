using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    BoxCollider2D boxCollider;
    Vector2 velocity;

    private float speed;
    private float acceleration;
    private bool isGrounded = false;

    [SerializeField]
    private LayerMask layerPlatform;
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
    float airSpeed;
    [SerializeField]
    float airAcceleration;

    [SerializeField]
    float jumpForce;
    [SerializeField]
    float jumpBufferTime;

    Collider2D[] hits;
    private bool jaiCognerLaTete = false;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        Debug.Assert(boxCollider != null, "Le joueur doit posséder une box de collision");
    }

    private void Update()
    {
        ComputeMovment();
        transform.Translate(velocity * Time.deltaTime);
        ComputeCollisions();
    }

    void ComputeMovment()
    {
        if (Input.GetAxisRaw("Run") > 0)
        {
            speed = runSpeed;
            acceleration = runAcceleration;
        }
        else
        {
            speed = walkSpeed;
            acceleration = walkAcceleration;
        }

        if (isGrounded)
        {
            velocity.y = 0;
            jaiCognerLaTete = false;
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        } else
        {
            speed = airSpeed;
            acceleration = airAcceleration;
            Debug.Log("Je suis en l'air");
            velocity.y += - gravity * Time.deltaTime;
        }

        if (!jaiCognerLaTete)
        {
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + boxCollider.size / 2, Vector2.up, 0.1f, layerPlatform);
            if (hit.collider != null)
            {
                velocity.y = 0.0f;
                jaiCognerLaTete = true;
            }
            
        }

        if (Input.GetButtonUp("Jump"))
        {
            velocity.y *= 0.5f;
        }
        
        float moveInput = Input.GetAxisRaw("Horizontal");
        velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInput, acceleration * Time.deltaTime);
    }

    //precondition le joueur a appuyé sur Jump et isGrounded
    void Jump()
    {
        velocity.y = jumpForce;
    }

    void ComputeCollisions()
    {
        hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);

        isGrounded = false;

        foreach (Collider2D hit in hits)
        {
            if (hit == boxCollider) { continue; } // On détecte forcement uen collision avec notre propre collider;

            ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

            if (colliderDistance.isOverlapped)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
                
                if(Vector2.Angle(colliderDistance.normal, Vector2.up) < 90)
                {
                    isGrounded = true;
                }
            }
        }
    }
}

