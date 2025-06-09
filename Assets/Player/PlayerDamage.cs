using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public int damage;

    [SerializeField]
    EnemyHp enemyHp;

    [SerializeField]
    PlayerMana playerMana;

    [SerializeField]
    float manaGain;

    private void Update()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Default");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemyHp = collision.gameObject.GetComponent<EnemyHp>();
            enemyHp.TakeDamage(damage);

            playerMana.currentMana += manaGain;
        }
    }
}
