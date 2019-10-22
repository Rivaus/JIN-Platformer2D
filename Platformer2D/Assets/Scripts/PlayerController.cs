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
    private bool isRunning = false;


    [Header("Collision et gravité")]
    [SerializeField]
    private LayerMask layerPlatform;
    [SerializeField]
    float gravity;

    BoxCollider2D boxCollider;
    Vector2 velocity;
    Collider2D[] hits;

    [Space]
    [Header("Element Visuel")]
    private Color basicColor;
    Material playerMaterial;
    [SerializeField]
    private Color doubleJumpUsedColor;

    [SerializeField]
    GameObject particule;
    bool wasGroundedLastFrame;
    float jumpTime;
    

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
    [SerializeField]
    float runJumpXFactor;
    [SerializeField]
    float runJumpYFactor;

    [Space]
    [Header("Son")]
    [SerializeField]
    AudioSource source;
    [SerializeField]
    AudioClip punchSound;
    [SerializeField]
    AudioClip jumpSound;

    [Space]
    [Header("Animation")]
    [SerializeField]
    Animator animator;



  
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        Debug.Assert(boxCollider != null, "Le joueur doit posséder une box de collision");
        basicColor = GetComponent<Renderer>().material.color;
        playerMaterial = GetComponent<Renderer>().material;

        animator.SetBool("hasFallen", false);
    }

    private void Update()
    {
        ComputeMovment();
        transform.Translate(velocity * Time.deltaTime);
        ComputeCollisions();
        if (doubleJumpUsed)
        {
           playerMaterial.color = doubleJumpUsedColor;
        }
        else
        {
           playerMaterial.color = basicColor;
        }
    }

    void ComputeMovment()
    {
        //Gestion de la vitesse en fonction de l'état
        if (Input.GetAxisRaw("Run") > 0)
        {
            if(velocity.x != 0)
            {
                isRunning = true;
            }
            else {
                isRunning = false;
            }

            speed = runSpeed;
            acceleration = runAcceleration;
        }
        else
        {
            isRunning = false;
            speed = walkSpeed;
            acceleration = walkAcceleration;
        }

        if (isGrounded)
        {
            velocity.y = 0;
            jaiCognerLaTete = false;
            isWallJumping = false;
            animator.SetBool("hasFallen", false);

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

            if (Input.GetButtonDown("Jump"))
            {
                if (isWallJumping)
                {
                    WallJump();
                    isWallJumping = false;
                } else
                {
                    RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, -Vector2.up, jumpDistanceTolerance, layerPlatform);
                    if (hit.collider != null)
                    {
                        wantToJump = true;
                    }

                    if (!doubleJumpUsed && !wantToJump)
                    {
                        Jump();
                        doubleJumpUsed = true;
                        isWallJumping = false;
                    }
                }
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
        source.clip = jumpSound;
        source.Play();
        if(isRunning)
        {
            velocity.y = jumpForce * runJumpYFactor;
            velocity.x = velocity.x * runJumpXFactor;
        }else {
            velocity.y = jumpForce;
        }
        
    }



    void WallJump()
    {
        source.clip = jumpSound;
        source.Play();

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

        wasGroundedLastFrame = isGrounded;

        isGrounded = false;

        // Gère les collisions avec le sol et les murs
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

                    if (isGrounded && !wasGroundedLastFrame && jumpTime > .5f)
                    {
                        // EN cas de chute haute :

                        //Spawn de particule
                        Instantiate(particule, transform.position, Quaternion.identity);

                        //Son joué
                        source.clip = punchSound;
                        source.Play();
                        
                        //Animation jouée
                        animator.SetBool("hasFallen", true);

                    }
                }

                if (hit.gameObject.CompareTag("Wall"))
                {
                    isWallJumping = true;
                    velocity.x = 0;
                }
            }
        }

        // Gère le temps de vol
        if (!isGrounded)
        {
            jumpTime += Time.deltaTime;
        } else
        {
            jumpTime = 0;
        }


        // On regarde si on touche les murs sur le côté
        RaycastHit2D wallHit = Physics2D.Raycast((Vector2)transform.position - boxCollider.size / 2, Vector2.left, .1f, layerPlatform);
        if (wallHit.collider != null)
        {
            contactWithLeftWall = true;
        } else
        {
            contactWithLeftWall = false;
        }
        wallHit = Physics2D.Raycast((Vector2)transform.position + boxCollider.size / 2, Vector2.right, .1f, layerPlatform);
        if (wallHit.collider != null)
        {
            contactWithRightWall = true;
        }
        else
        {
            contactWithRightWall = false;
        }

        //On regarde si on est sur une platforme particulière;

        RaycastHit2D platformHit = Physics2D.Raycast((Vector2)transform.position, Vector2.down, 1, layerPlatform);
        if (platformHit.collider != null)
        {
            if (platformHit.collider.CompareTag("Special Platform"))
            {
                platformHit.collider.gameObject.GetComponent<SpecialPlatform>().OnActivate();
            }
        }
    }
}

