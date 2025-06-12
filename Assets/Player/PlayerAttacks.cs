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


    bool canAttack = true;
    private bool isAttacking;


    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void Update()
    {
        //this.gameObject.layer = LayerMask.NameToLayer("Default");
        PreAttack();
        collider.layer = LayerMask.NameToLayer("Default");
    }

    void PreAttack()
    {
        if (Input.GetKeyDown(attackKey) && canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        canAttack = false;
        attack.SetActive(true);
        collider.SetActive(true);

        yield return new WaitForSeconds(0.2f);

            attack.SetActive(false);
            collider.SetActive(false);

        yield return new WaitForSeconds(0.3f);
        canAttack = true;

        yield return null;
    }
}
