using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerInteraction : TriggerInteractionBase
{
    public enum doorToSpawnAt
    {
        None,
        One,
        Two,
        Three,
        Four,
    }

    [Header("Spawn To")]
    [SerializeField] private doorToSpawnAt DoorToSpawnTo;
    [SerializeField] private SceneField sceneToLoad;

    [Space(10f)]
    [Header("This Door")]
    public doorToSpawnAt CurrentDoorPosition;

    public override void Interact()
    {
        SceneSwapManager.SwapSceneFromDoorUse(sceneToLoad, DoorToSpawnTo);
    }
}
