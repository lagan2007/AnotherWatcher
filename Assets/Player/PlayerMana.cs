using Unity.VisualScripting;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    public float maxMana;
    public float currentMana;

    private void Update()
    {
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
    }
}
