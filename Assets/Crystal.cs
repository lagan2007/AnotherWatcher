using UnityEngine;

public class Crystal : MonoBehaviour
{

    public GameObject player;
    public PlayerController playerController;
    public GameObject saveObj;
    public Save save;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        saveObj = GameObject.FindGameObjectWithTag("SaveManager");
        playerController = player.GetComponent<PlayerController>();
        save = saveObj.GetComponent<Save>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController != null)
        {
            if (playerController.dashObtained)
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            playerController = collision.GetComponent<PlayerController>();
            //save = collision.gameObject.gameObject.GetComponent<Save>();
            playerController.dashObtained = true;
            StartCoroutine(save.SaveCoroutine());
        }
        
    }
}
