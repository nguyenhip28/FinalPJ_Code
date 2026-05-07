using UnityEngine;

public class NPCEnterShopTrigger : MonoBehaviour
{
    public DoorInteractable door;
    public NPCQueueManager queueManager;
    public Transform wp_order;
    public Transform wp_out;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("NPC")) return;

        NPCMovement npc = other.GetComponent<NPCMovement>();
        if (npc == null) return;

        if (door.IsOpen())
        {
            bool joined = queueManager.TryJoinQueue(npc);

            if (joined)
            {
                npc.EnterShop(queueManager, wp_order, wp_out);
            }
        }
    }
}