using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapManager : MonoBehaviour
{
    public static SceneSwapManager instance;

    private static bool loadFromDoor;

    private GameObject player;


    private float doorOffset;

    private Transform doorTransform;
    private Vector3 playerSpawnPositon;

    private DoorTriggerInteraction.doorToSpawnAt _doorToSpawnto;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }




    public static void SwapSceneFromDoorUse(SceneField myScene, DoorTriggerInteraction.doorToSpawnAt doorToSpawnAt)
    {
        loadFromDoor = true;
        instance.StartCoroutine(instance.FadeOutThenChangeScene(myScene, doorToSpawnAt));
    }

    private IEnumerator FadeOutThenChangeScene(SceneField myScene, DoorTriggerInteraction.doorToSpawnAt doorToSpawnAt = DoorTriggerInteraction.doorToSpawnAt.None)
    {
        SceneFadeManager.instance.StartFadeut();

        while (SceneFadeManager.instance.isFadingOut)
        {
            yield return null;
        }

        //keep fading out
        

        _doorToSpawnto = doorToSpawnAt;
        SceneManager.LoadScene(myScene);
    }

    //called whenever scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneFadeManager.instance.StartFadeIn();

        if (loadFromDoor)
        {
            FindDoor(_doorToSpawnto);
            player.transform.position = playerSpawnPositon;

            loadFromDoor = false;
        }
    }

    private void FindDoor(DoorTriggerInteraction.doorToSpawnAt doorSpawnNumber)
    {
        DoorTriggerInteraction[] doors = FindObjectsOfType<DoorTriggerInteraction>();

        for (int i = 0; i < doors.Length; i++)
        {
            if (doors[i].CurrentDoorPosition == doorSpawnNumber)
            {
                doorTransform = doors[i].gameObject.GetComponent<Transform>();

                CalculateSpawnPosition();
                return;
            }

        }
    }

    private void CalculateSpawnPosition()
    {
        playerSpawnPositon = new Vector3 (doorTransform.position.x, doorTransform.position.y, 0f);
    }
}
