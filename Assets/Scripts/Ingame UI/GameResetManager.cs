using UnityEngine;

public class GameResetManager : MonoBehaviour
{
    [Header("NPC")]
    public Transform npcContainer;
    public NPCQueueManager queueManager;

    [Header("Door")]
    public DoorSwitch doorSwitch;
    public DoorInteractable doorState;

    public void ResetDay()
    {
        
        foreach (Transform child in npcContainer)
        {
            Destroy(child.gameObject);
        }

        
        queueManager.ClearQueue();

       
        if (doorState.IsOpen())
        {
            doorSwitch.ToggleBothDoors();
        }
    }
}