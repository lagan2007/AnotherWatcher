using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInteractionBase : MonoBehaviour, IInteractible
{
    public GameObject player { get; set; }


    public bool canInteract {  get; set; }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (canInteract)
        {
            //if  (Input.GetKeyDown(KeyCode.F))
            //{
                //Interact();
            //}
        }
    }

    public virtual void Interact()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            canInteract = true;
            //This is modified to interact on contact
            Interact();
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            canInteract = false;
        }
    }
}
