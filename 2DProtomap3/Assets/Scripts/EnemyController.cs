using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    bool isInvincible;      // Ob man schaden nimmt oder nicht

    public int currentHealth;
    public int maxHealth = 1;
    public int contactDamage = 1;       //Wenn Rayze auf ein Gegner trifft, dann kriegen wir schade


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    
    public void Invincible(bool inviciblity)
    {
        isInvincible = inviciblity;
    }

    public void TakeDamage(int damage)
    {
        if(!isInvincible)
        {
            currentHealth -= damage;
            Mathf.Clamp(currentHealth, 0, maxHealth); // Die Range, dass es niemals unter 0 geht und über den maxHealth
            if(currentHealth <= 0)
            {
                Defeat();
            }
        }
    }

    void Defeat()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            player.HitSide(transform.position.x > player.transform.position.x);     // Auf welcher Seiter der Player getrpffen wird
            player.TakeDamage(this.contactDamage);
        }
    }
}
