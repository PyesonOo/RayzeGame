using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiMove : MonoBehaviour
{

    public float speed = 40f;
    public float circleRadius;
    public GameObject groundCheck;
    public LayerMask groundLayer;
    public bool isFacingRight;
    public bool isGrounded;
    private Rigidbody2D EnemyRB;


    // Start is called before the first frame update
    void Start()
    {
        EnemyRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void Flip(){

        isFacingRight = !isFacingRight;
        transform.Rotate(new Vector3(0, 180, 0));
        speed = -speed;
    }

    private void OnDrawGizmosSelected(){

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.transform.position,circleRadius);
    }


    void FixedUpdate(){
        

        EnemyRB.velocity = Vector2.right * speed * Time.deltaTime;
        
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position,circleRadius,groundLayer);

        if(!isGrounded && isFacingRight ){

            Flip();
        }
        else if(!isGrounded && !isFacingRight){

            Flip();
        }


    }

}
