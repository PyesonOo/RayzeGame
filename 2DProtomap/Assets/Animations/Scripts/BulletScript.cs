using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    Rigidbody2D rb2d;
    SpriteRenderer sprite;

    float destroyTime;

    public int damage = 1; //Schaden

    [SerializeField] float bulletSpeed;     //Geschwindigkeit
    [SerializeField] Vector2 bulletDirection;       //Richtung
    [SerializeField] float destroayDelay;       //Wann unser Schuss zerstört wird

    // Start is called before the first frame update
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        destroyTime -= Time.deltaTime;
        if(destroyTime < 0)
        {
            Destroy(gameObject);  //Der Schuss wird nach einer Zeit zerstört
        }

    }

    public void SetBulletSpeed(float speed)
    {
        this.bulletSpeed = speed;
    }

    public void SetBulletDirection(Vector2 direction)
    {
        this.bulletDirection = direction;
    }

    public void SetDamageValue ( int damage)
    {
        this.damage = damage;
    }

    public void SetDestroyDelay(float delay)
    {
        this.destroayDelay = delay;
    }

    public void Shoot()
    {
        sprite.flipX = (bulletDirection.x < 0);     // Den Sprite Flippen, wenn wir nach rechts oder links schießen
        rb2d.velocity = bulletDirection * bulletSpeed;
        destroyTime = destroayDelay;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Destroy(gameObject);   //Wenn unser Schuss was berührt, dann wird es zerstört
        
    }
}
