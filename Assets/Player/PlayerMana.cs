using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : MonoBehaviour
{
    public float maxMana;
    public float currentMana;

    public Slider slider;

    private void Update()
    {
        slider.value = currentMana;
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
    }
}
