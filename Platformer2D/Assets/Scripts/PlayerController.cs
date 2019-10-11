using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   
    // Variable d'états
    private float speed;
    private float acceleration;
    private bool isGrounded = false;
    private bool doubleJumpUsed = false;
    private bool wantToJump = false;
    private bool isWallJumping = false;
    private bool contactWithRightWall = false;
    private bool contactWithLeftWall = false;
    private bool jaiCognerLaTete = false;


    [Header("Collision et gravité")]
    [SerializeField]
    private LayerMask layerPlatform;
    [SerializeField]
    float gravity;

    BoxCollider2D boxCollider;
    Vector2 velocity;
    Collider2D[] hits;
  



    [Space]
    [Header("Mouvement")]
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

    [Header("Saut")]
    [SerializeField]
    float jumpForce;
    [SerializeField]
    float jumpDistanceTolerance;
    [SerializeField]
    float wallJumpXFactor;
    [SerializeField]
    float wallJumpYFactor;
   

  
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
        //Gestion de la vitesse en fonction de l'état
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
            isWallJumping = false;

            if (Input.GetButtonDown("Jump") || wantToJump)
            {
                Jump();
                wantToJump = false;
            }
        } else
        {
            speed = airSpeed;
            acceleration = airAcceleration;
            
            velocity.y += - gravity * Time.deltaTime;

            if (Input.GetButtonDown("Jump") && isWallJumping)
            {
                WallJump();
                isWallJumping = false;
            }
            //Want to jump but isnt grounded, allow a bit of time to jump a bit after.
            else if (Input.GetButtonDown("Jump") && doubleJumpUsed)
            {
                RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position - boxCollider.size / 2, -Vector2.up, jumpDistanceTolerance, layerPlatform);
                if (hit.collider != null)
                {
                    wantToJump = true;
                }
            } // Double jump
            else if (Input.GetButtonDown("Jump") && !doubleJumpUsed && !wantToJump)
            {
                Jump();
                doubleJumpUsed = true;
                isWallJumping = false;
            }
        }

        //Permet la gestion des plafonds
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

    void WallJump()
    {

        if (contactWithLeftWall) // Quand on wall jump, c'est forcément sur le côté, sinon, on pourrait monter à l'infini    
        {
            velocity.y = jumpForce * wallJumpYFactor;
            velocity.x = speed * wallJumpXFactor;
        }
        else if (contactWithRightWall)
        {
            velocity.y = jumpForce * wallJumpYFactor;
            velocity.x = -speed * wallJumpXFactor;
        }
    }

    //la dedans il y a le test de si on touche le sol
    void ComputeCollisions()
    {
        hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);

        isGrounded = false;

        foreach (Collider2D hit in hits)
        {
            if (hit == boxCollider) { continue; } // On détecte forcement une collision avec notre propre collider;

            ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

            if (colliderDistance.isOverlapped)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
               
                if(Vector2.Angle(colliderDistance.normal, Vector2.up) < 90)
                {
                    isGrounded = true;
                    doubleJumpUsed = false;
                }

                if (hit.gameObject.CompareTag("Wall"))
                {
                    isWallJumping = true;
                    velocity.x = 0;
                }
            }
        }

        // On regarde si on touche les murs sur le côté
        RaycastHit2D wallHit = Physics2D.Raycast((Vector2)transform.position - boxCollider.size / 2, Vector2.left, 1f, layerPlatform);
        if (wallHit.collider != null)
        {
            contactWithLeftWall = true;
        } else
        {
            contactWithLeftWall = false;
        }
        wallHit = Physics2D.Raycast((Vector2)transform.position + boxCollider.size / 2, Vector2.right, 1f, layerPlatform);
        if (wallHit.collider != null)
        {
            contactWithRightWall = true;
        }
        else
        {
            contactWithRightWall = false;
        }
    }
}

