using UnityEngine;

public class Load : MonoBehaviour
{
    GameObject saveManager;
    Save save;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveManager = GameObject.FindGameObjectWithTag("SaveManager");
        save = saveManager.GetComponent<Save>();
        StartCoroutine(save.LoadData());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
