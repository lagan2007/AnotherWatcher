using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    [SerializeField]
    public int maxHp;

    [SerializeField]
    Save save;

    [SerializeField]
    GameObject wakeUpPanel;

    public Slider slider;

    public int currentHp;

    public bool hasRun = false;

    public bool hasDied = false;


    private void Start()
    {
        currentHp = maxHp;
    }

    private void Update()
    {
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
        slider.value = currentHp;
        if (currentHp <= 0 && !hasRun)
        {
            StartCoroutine(Die());
            hasRun = true;
        }
    }

    IEnumerator Die()
    {
        wakeUpPanel.SetActive(true);
        yield return new WaitForSeconds(1.20f);
        wakeUpPanel.SetActive(false);
        hasDied = true;
        //gameObject.active = false;
        StartCoroutine(save.LoadData());
        yield return null;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
    }
}
