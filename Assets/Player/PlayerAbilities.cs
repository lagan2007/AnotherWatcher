using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [SerializeField]
    PlayerMana playerMana;

    [SerializeField]
    PlayerHp playerHp;

    [SerializeField]
    KeyCode healKey;

    float manaSpentToHeal;

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
            playerMana.currentMana -= Time.deltaTime * 15;
            manaSpentToHeal += Time.deltaTime * 15;
        }
        else
        {
            manaSpentToHeal = 0;
        }

        if (manaSpentToHeal >= 30)
        {
            playerHp.currentHp += 1;
            manaSpentToHeal = 0;
        }

        //Debug.Log(playerMana.currentMana);
    }
}
