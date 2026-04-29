using System.Collections.Generic;
using UnityEngine;

public class NPCQueueManager : MonoBehaviour
{
    [Header("Queue Settings")]
    public Transform orderPoint;
    public Transform exitPoint;

    public Transform[] queuePoints; // vị trí xếp hàng
    public int maxNPCInShop = 3;    // 🔥 GIỚI HẠN

    private Queue<NPCMovement> queue = new Queue<NPCMovement>();

    // ================= JOIN =================
    public bool TryJoinQueue(NPCMovement npc)
    {
        if (queue.Count >= maxNPCInShop)
        {
            return false; // ❌ full → không vào
        }

        queue.Enqueue(npc);

        UpdateQueuePositions();

        return true;
    }

    // ================= CHECK FIRST =================
    public bool IsFirst(NPCMovement npc)
    {
        return queue.Count > 0 && queue.Peek() == npc;
    }

    // ================= FINISH ORDER =================
    public void FinishOrder(NPCMovement npc)
    {
        if (queue.Count == 0) return;

        if (queue.Peek() == npc)
        {
            queue.Dequeue();

            // 👉 cho NPC đi ra
            npc.GoToExit(exitPoint);

            // 👉 update lại queue
            UpdateQueuePositions();
        }
    }

    // ================= UPDATE POSITIONS =================
    void UpdateQueuePositions()
    {
        int index = 0;

        foreach (var npc in queue)
        {
            if (index == 0)
            {
                // 🔥 NPC đầu → đi order
                npc.GoToOrder(orderPoint);
            }
            else
            {
                int queueIndex = index - 1;

                if (queueIndex < queuePoints.Length)
                {
                    npc.SetQueuePosition(queuePoints[queueIndex]);
                }
            }

            index++;
        }
    }

    public void ClearQueue()
    {
        queue.Clear();
    }

    public NPCMovement GetFirstNPC()
    {
        if (queue.Count == 0) return null;
        return queue.Peek();
    }
}