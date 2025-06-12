using System.Collections;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [SerializeField]
    PlayerMana playerMana;

    [SerializeField]
    PlayerHp playerHp;

    [SerializeField]
    KeyCode healKey;

    [SerializeField]
    ParticleSystem particleHeal;

    [SerializeField]
    ParticleSystem particle;

    float manaSpentToHeal;

    bool hasRun = false;
    bool hasRun2 = false;

    private void Start()
    {
        playerHp = gameObject.GetComponent<PlayerHp>();
        playerMana = gameObject.GetComponent<PlayerMana>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(healKey) && playerMana.currentMana > 0)
        {
            playerMana.currentMana -= Time.deltaTime * 20;
            manaSpentToHeal += Time.deltaTime * 20;
            if (!hasRun)
            {
                particleHeal.Play();
                hasRun = true;
            }
            hasRun2 = false;
            
        }
        else
        {
            //manaSpentToHeal = 0;
            if (!hasRun2)
            {
                particleHeal.Stop();
                hasRun2 = true;
            }

            hasRun = false;
            
        }

        if (manaSpentToHeal >= 30)
        {
            playerHp.currentHp += 1;
            particle.Play();
            manaSpentToHeal = 0;
        }



        //Debug.Log(playerMana.currentMana);
    }
}
