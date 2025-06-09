using System.Collections;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage;

    [SerializeField]
    PlayerHp playerHp;

    [SerializeField]
    PlayerController playerController;

    public Rigidbody2D playerBody;

    public GameObject player;

    bool playerIsOnRight;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerController = collision.gameObject.GetComponent<PlayerController>();
            playerHp = collision.gameObject.GetComponent<PlayerHp>();
            playerBody = collision.gameObject.GetComponent<Rigidbody2D>();
            player = collision.gameObject.GetComponent<GameObject>();
            playerHp.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerHp = collision.gameObject.GetComponent<PlayerHp>();
            playerController = collision.gameObject.gameObject.GetComponent<PlayerController>();
            playerBody = collision.gameObject.GetComponent<Rigidbody2D>();
            player = collision.gameObject;
            if (playerHp.currentHp > 0)
            {
                playerController.hasIFrames = true;
                player.layer = LayerMask.NameToLayer("Invincible");
            }
            playerHp.TakeDamage(damage);
            CheckSides();
            playerController.hitParticles.Play();

            StartCoroutine(Knockback());
            
        }

        
    }

    private void CheckSides()
    {
        if (this.transform.position.x > player.transform.position.x)
        {
            playerIsOnRight = false;
        }
        else
        {
            playerIsOnRight = true;
        }
    }

    float knockbackTime = 0.2f;

    IEnumerator Knockback()
    {
        float timer = 0;

        playerController.canMove = false;
        
        while (timer <= knockbackTime)
        {
            if (playerIsOnRight)
            {
                playerBody.linearVelocity = new Vector2(15, 15);
            }
            else
            {
                playerBody.linearVelocity = new Vector2(-15, 15);
            }
            

            
            
            
            playerController.virtualCameraPerlin.m_AmplitudeGain = 20;
            timer = timer + Time.deltaTime;
            yield return null;
        }

        if (playerHp.currentHp > 0)
        {
            StartCoroutine(playerController.flicker());
        }
        

        yield return new WaitForSeconds(0.2f);
        playerController.virtualCameraPerlin.m_AmplitudeGain = 0;
        playerController.canMove = true;

        yield return new WaitForSeconds(1.8f);
        player.layer = LayerMask.NameToLayer("Default");
        playerController.hasIFrames = false;
    }

    
}
