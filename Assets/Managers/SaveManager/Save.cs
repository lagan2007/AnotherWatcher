using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Save : MonoBehaviour
{
    public Scene scene;

    public Transform playerTransform;

    [SerializeField]
    PlayerController playerController;


    [SerializeField]
    GameObject player;

    [SerializeField]
    PlayerHp playerHp;

    [SerializeField]
    SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SaveData();
        LoadData();
    }

    public void LoadData()
    {
        if(Input.GetKeyDown(KeyCode.L) || playerHp.hasDied)
        {
            string json = File.ReadAllText(Application.dataPath + "/playerData.json");
            DataToSave dataToSave = JsonUtility.FromJson<DataToSave>(json);
            //load scene
            SceneManager.LoadScene(dataToSave.sceneName);
            //load position
            playerTransform.position = dataToSave.playerPosition;
            //set player to alive
            player.active = dataToSave.alive;
            //load hp
            playerHp.currentHp = dataToSave.hp;
            //load if player can dash
            playerController.dashObtained = dataToSave.dashObtained;
            //debug invincibility
            player.layer = LayerMask.NameToLayer("Default");
            playerController.hasIFrames = false;
            //reset death
            playerHp.hasRun = false;
            playerHp.hasDied = false;
            //debug visuals
            spriteRenderer.sortingOrder = 1;
        }
        
    }

    private void SaveData()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            //get and save scene
            scene = SceneManager.GetActiveScene();
            DataToSave dataToSave = new DataToSave();
            dataToSave.sceneName = scene.name;
            //save position of player
            dataToSave.playerPosition = playerTransform.position;
            //set player to alive
            dataToSave.alive = true;
            //full heal and save hp
            playerHp.currentHp = playerHp.maxHp;
            dataToSave.hp = playerHp.currentHp;
            //can player Dash?
            dataToSave.dashObtained = playerController.dashObtained;

            string json = JsonUtility.ToJson(dataToSave);
            File.WriteAllText(Application.dataPath + "/playerData.json", json);

            Debug.Log("saved");
        }
        
    }

    
}
