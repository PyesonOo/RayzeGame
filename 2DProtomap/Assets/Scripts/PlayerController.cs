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




    public float groundCheckRadius;
    public LayerMask whatIsGround;

    public Transform groundCheck;

    float keyHorizontal;
    bool keyJump;
    bool keyShoot;

    bool isGrounded;
    bool isShooting;
    bool isFacingRight;   // In welcher Richtung wir gucken

    float shootTime;
    bool keyShootRelease;

    void Start()
    {
        animator = GetComponent<Animator>();
        box2d = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();


        isFacingRight = true; // Char guckt nach rechts
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

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius, whatIsGround);
    }

    void Update()
    {
        //keyHorizontal = Input.GetAxisRaw("Horizontal");
        //keyJump = Input.GetKeyDown(KeyCode.Space);
        //keyShoot = Input.GetKey(KeyCode.Mouse0);

        //isShooting = keyShoot;

        PlayerDirectionInput();
        PlayerJumpInput();
        PlayerShootInput();
        PlayerMovement();
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

    

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position,groundCheckRadius);
    }


}
