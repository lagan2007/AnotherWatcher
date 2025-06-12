using UnityEngine;

public class Crystal : MonoBehaviour
{

    GameObject player;
    PlayerController playerController;
    Save save;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
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
            playerController.dashObtained = true;
            StartCoroutine(save.SaveCoroutine());
        }
        
    }
}
