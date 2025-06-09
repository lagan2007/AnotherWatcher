using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHp : MonoBehaviour
{
    [SerializeField]
    public int maxHp;

    [SerializeField]
    Save save;

    [SerializeField]
    GameObject wakeUpPanel;

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
        save.LoadData();
        yield return null;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
    }
}
