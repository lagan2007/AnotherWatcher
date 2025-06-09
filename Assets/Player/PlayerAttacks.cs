using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Build.Content;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    [SerializeField]
    private GameObject attack;

    [SerializeField]
    KeyCode attackKey;

    [SerializeField]
    float attackTimeCounter;

    [SerializeField]
    float attackTime;
    private bool isAttacking;


    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    private void Attack()
    {
        if (Input.GetKeyDown(attackKey)) 
        {
            isAttacking = true;
            attackTimeCounter = attackTime;
        }

        if(isAttacking && attackTimeCounter > 0)
        {
            attack.SetActive(true);
            attackTimeCounter -= Time.deltaTime;

        }
        else
        {
            attack.SetActive(false);
        }
    }
}
