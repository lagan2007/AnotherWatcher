using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Build.Content;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    [SerializeField]
    private GameObject attack;

    [SerializeField]
    private GameObject collider;

    [SerializeField]
    KeyCode attackKey;

    [SerializeField]
    float attackTimeCounter;

    [SerializeField]
    float attackTime;

    float cooldown;
    private bool isAttacking;


    // Update is called once per frame
    void FixedUpdate()
    {
        Attack();
    }

    private void Update()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Default");
        collider.layer = LayerMask.NameToLayer("Default");
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
            collider.SetActive(true);
            attackTimeCounter -= Time.deltaTime;
            cooldown = 0;
        }
        else
        {
            attack.SetActive(false);
            collider.SetActive(false);
        }
    }
}
