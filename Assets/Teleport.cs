using UnityEngine;

public class Teleport : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    Transform tp;
    [SerializeField]
    public int damage;

    PlayerHp playerHp;
    GameObject player;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        player = collision.gameObject;
        playerHp = player.GetComponent<PlayerHp>();
        collision.gameObject.transform.position = tp.position;
        playerHp.TakeDamage(damage);
    }
}
