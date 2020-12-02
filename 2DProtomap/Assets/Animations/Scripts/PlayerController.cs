using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    
    Animator animator;
    BoxCollider2D box2d;
    Rigidbody2D rb2d;

    [SerializeField] float moveSpeed = 3f; //Bewegungs geschwindigkeit
    [SerializeField] float jumpSpeed = 3f; // Sprungkraft
    [SerializeField] int bulletDamage = 1;
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] Transform bulletShootPos;
    [SerializeField] GameObject bulletPrefab;


    public float wallCheckDistance;
    public float wallSlidingSpeed;
    public float groundCheckRadius;
    public LayerMask whatIsGround;

    public Transform groundCheck;
    public Transform wallCheck;

    float keyHorizontal;
    bool keyJump;
    bool keyShoot;
    
    

    bool isGrounded;
    bool isTouchingWall;
    bool isWallSliding;
    bool isShooting;
    bool isTakingDamage;
    bool isInvincible;
    bool isFacingRight;   // In welcher Richtung wir gucken

    bool hitSideRight;      // Ob wir von der Rechten oder Linken Seite getroffen werden, für die Hit Animation

    float shootTime;
    bool keyShootRelease;

    public int currentHealth;
    public int maxHealth = 28;

    void Start()
    {
        animator = GetComponent<Animator>();
        box2d = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();


        isFacingRight = true; // Char guckt nach rechts

        currentHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        
        CheckSurroundings();
        //isGrounded = false;
        //Color raycastColor;
        //RaycastHit2D raycastHit;
        //float raycastDistance = 0.05f;
        //int layerMask = 1 << LayerMask.NameToLayer("Ground");
        //ground check
        //Vector3 box_origin = box2d.bounds.center;
        //box_origin.y = box2d.bounds.min.y + (box2d.bounds.extents.y / 4f);
        //Vector3 box_size = box2d.bounds.size;
        //box_size.y = box2d.bounds.size.y / 4f;
        //raycastHit = Physics2D.BoxCast(box_origin, box_size, 0f, Vector2.down, raycastDistance, layerMask);
         //Wenn der Player auf den Ground trifft
            //if (raycastHit.collider != null)
            //{
                //isGrounded = true;
            //}
        //Visuelle Zeichnung des Colliders : Grün= boden, rot= in der Luft
            //raycastColor = (isGrounded) ? Color.green : Color.red;
            //Debug.DrawRay(box_origin + new Vector3(box2d.bounds.extents.x, 0), Vector2.down * (box2d.bounds.extents.y / 4f + raycastDistance), raycastColor);
            //Debug.DrawRay(box_origin - new Vector3(box2d.bounds.extents.x, 0), Vector2.down * (box2d.bounds.extents.y / 4f + raycastDistance), raycastColor);
            //Debug.DrawRay(box_origin - new Vector3(box2d.bounds.extents.x, box2d.bounds.extents.y / 4f + raycastDistance), Vector2.right * (box2d.bounds.extents.x * 2), raycastColor);
    }

    private void CheckIfWallSliding()
    {
        if(isTouchingWall && !isGrounded && rb2d.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
        

    }


    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
    }

    void Update()
    {
       
        //keyHorizontal = Input.GetAxisRaw("Horizontal");
        //keyJump = Input.GetKeyDown(KeyCode.Space);
        //keyShoot = Input.GetKey(KeyCode.Mouse0);

        //isShooting = keyShoot;

        if(isTakingDamage)
        {
            animator.Play("Ryze_Hit");
            return;
        }


        PlayerDirectionInput();
        PlayerJumpInput();
        PlayerShootInput();
        PlayerMovement();
        CheckIfWallSliding();
    }

    void PlayerDirectionInput()
    {
        keyHorizontal = Input.GetAxisRaw("Horizontal");
        
    }

    void PlayerJumpInput()
    {
        keyJump = Input.GetKeyDown(KeyCode.Space);
    }

    void PlayerShootInput()
    {
        keyShoot = Input.GetKey(KeyCode.Mouse0);
        float shootTimeLenght = 0;
        float keyShootReleaseTimeLenght = 0;

        if(keyShoot && keyShootRelease)
        {
            isShooting = true;
            keyShootRelease = false;
            shootTime = Time.time;
            // Den Schuss abfeuern
            Invoke("ShootBullet", 0.1f);
        }
        if(!keyShoot && !keyShootRelease)
        {
            keyShootReleaseTimeLenght = Time.time - shootTime;
            keyShootRelease = true;
        }
        if(isShooting)
        {
            shootTimeLenght = Time.time - shootTime;
            if(shootTimeLenght >= 0.25f || keyShootReleaseTimeLenght >= 0.15f) // Die Zeit, wie lange wir schießen können
            {
                isShooting = false;          
            }
        }
    }

    void PlayerMovement()
    {
        if (keyHorizontal < 0)
        {
            if (isFacingRight)
            {
                Flip();
            }
            if (isGrounded)
            {
                if (isShooting)
                {
                    animator.Play("Ryze_RunShoot");
                }
                else
                {
                    animator.Play("Ryze_Run");

                }
            }
            rb2d.velocity = new Vector2(-moveSpeed, rb2d.velocity.y);
        }
        else if (keyHorizontal > 0)
        {
            if (!isFacingRight)
            {
                Flip();
            }
            if (isGrounded)
            {
                if (isShooting)
                {
                    animator.Play("Ryze_RunShoot");
                }
                else
                {
                    animator.Play("Ryze_Run");

                }
            }
            rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
        }
        else
        {
            if (isGrounded)
            {
                if (isShooting)
                {
                    animator.Play("Ryze_Shoot");
                }
                else
                {
                    animator.Play("Ryze_Idle");

                }
            }
            rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
        }
        if (keyJump && isGrounded)
        {
            if (isShooting)
            {
                animator.Play("Ryze_JumpShoot");
            }
            else
            {
                animator.Play("Ryze_Jump");

            }
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);

        }

        if (!isGrounded)
        {
            if (isShooting)
            {
                animator.Play("Ryze_JumpShoot");

            }
            else
            {
                animator.Play("Ryze_Jump");
            }

        }

        if(isWallSliding)
        {
            if(rb2d.velocity.y < wallSlidingSpeed)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, -wallSlidingSpeed);
            }
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0);
    }

    void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletShootPos.position, Quaternion.identity);
        bullet.name = bulletPrefab.name;
        bullet.GetComponent<BulletScript>().SetDamageValue(bulletDamage);
        bullet.GetComponent<BulletScript>().SetBulletSpeed(bulletSpeed);
        bullet.GetComponent<BulletScript>().SetBulletDirection((isFacingRight) ? Vector2.right : Vector2.left);
        bullet.GetComponent<BulletScript>().Shoot();



     }

    public void HitSide(bool rightSide)
    {
        hitSideRight = rightSide;
    }

    public void Invicible(bool invincibility)
    {
        isInvincible = invincibility;
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;
            Mathf.Clamp(currentHealth, 0, maxHealth); // Die Range, dass es niemals unter 0 geht und über den maxHealth
            if (currentHealth <= 0)
            {
                Defeat();
            }
            else
            {
                StartDamageAnimation();
            }
        }
    }

    void StartDamageAnimation()
    {
        if(!isTakingDamage)
        {
            isTakingDamage = true;   //Wenn wir noch keinen schaden nehmen, dann können wir schaden nehmen
            isInvincible = true;
            float hitForceX = 0.50f;
            float hitForceY = 1.5f;

            if (hitSideRight) hitForceX = -hitForceX;
            rb2d.velocity = Vector2.zero;
            rb2d.AddForce(new Vector2(hitForceX, hitForceY), ForceMode2D.Impulse);
        }
    }

    void StopDamageAnimation()
    {
        isTakingDamage = false;
        isInvincible = false;
        animator.Play("Ryze_Hit", -1, 0f);
    }

    void Defeat()
    {
        Destroy(gameObject);        //Rayze wird zerstört
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position,groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }


}
