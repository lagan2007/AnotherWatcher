using System.Collections;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField]
    public int damage = 1;

    [SerializeField]
    PlayerHp playerHp;

    [SerializeField]
    public PlayerController playerController;

    public GameObject player;



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {


            playerHp = collision.gameObject.GetComponent<PlayerHp>();
            playerController = collision.gameObject.gameObject.GetComponent<PlayerController>();
            player = collision.gameObject;

            playerHp.TakeDamage(damage);

            playerController.hitParticles.Play();
            CheckSides();
            StartCoroutine(playerController.Knockback());

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {


            playerHp = collision.gameObject.GetComponent<PlayerHp>();
            playerController = collision.gameObject.gameObject.GetComponent<PlayerController>();
            player = collision.gameObject;
            
            playerHp.TakeDamage(damage);
            
            playerController.hitParticles.Play();
            CheckSides();
            StartCoroutine(playerController.Knockback());
            
        }

        
    }

    private void CheckSides()
    {
        if (this.transform.position.x > player.transform.position.x)
        {
            playerController.playerIsOnRight = false;
        }
        else
        {
            playerController.playerIsOnRight = true;
        }
    }
    /*
    float knockbackTime = 0.2f;

    public IEnumerator Knockback()
    {
        if (playerHp.currentHp > 0)
        {
            playerController.hasIFrames = true;
            player.layer = LayerMask.NameToLayer("Invincible");
        }

        

        float kTimer = 0;

        playerController.canMove = false;
        
        while (kTimer <= knockbackTime)
        {
            if (playerIsOnRight)
            {
                playerBody.linearVelocity = new Vector2(15, 15);
            }
            else
            {
                playerBody.linearVelocity = new Vector2(-15, 15);
            }
            

            
            
            
            playerController.virtualCameraPerlin.m_AmplitudeGain = 5;
            kTimer = kTimer + Time.deltaTime;
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

        playerController.hasIFrames = false;
        player.layer = LayerMask.NameToLayer("Default");

        
        yield return null;
    }
    */


    
}
