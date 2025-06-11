using UnityEngine;
using System.Collections;

public class Boss2 : MonoBehaviour
{
    [SerializeField]
    Animator animator;


    int attack;
    int lastNumber;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Choose());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Choose()
    {
        attack = Random.Range(1, 4);
        if (lastNumber == attack)
        {
            StartCoroutine(Choose());
        }
        else
        {
            lastNumber = attack;
            StartCoroutine(Attack());
            yield return null;
        }

    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.25f);
        if (attack == 1)
        {
            animator.Play("Shoot");
        }
        else if (attack == 2)
        {
            animator.Play("Swing");
            yield return new WaitForSeconds(1.2f);

        }
        else if (attack == 3)
        {
            animator.Play("Slash");
        }
        yield return null;
    }
}
