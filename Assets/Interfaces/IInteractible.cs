using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractible
{
    public GameObject player { get; set;  }

    bool canInteract {  get; set; }

    void Interact();
}
