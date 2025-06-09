using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    
    [SerializeField]
    int maxHp;

    public int currentHp;

    private void Awake()
    {
        currentHp = maxHp;
    }

    private void Update()
    {
        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
    }
}


