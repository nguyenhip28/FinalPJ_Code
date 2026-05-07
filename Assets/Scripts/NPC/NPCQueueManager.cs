using System.Collections.Generic;
using UnityEngine;

public class NPCQueueManager : MonoBehaviour
{
    [Header("Queue Settings")]
    public Transform orderPoint;
    public Transform exitPoint;

    public Transform[] queuePoints; 
    public int maxNPCInShop = 3;    

    private Queue<NPCMovement> queue = new Queue<NPCMovement>();


    public bool TryJoinQueue(NPCMovement npc)
    {
        if (queue.Count >= maxNPCInShop)
        {
            return false; 
        }

        queue.Enqueue(npc);

        UpdateQueuePositions();

        return true;
    }


    public bool IsFirst(NPCMovement npc)
    {
        return queue.Count > 0 && queue.Peek() == npc;
    }


    public void FinishOrder(NPCMovement npc)
    {
        if (queue.Count == 0) return;

        if (queue.Peek() == npc)
        {
            queue.Dequeue();

            
            npc.GoToExit(exitPoint);

            
            UpdateQueuePositions();
        }
    }

    
    void UpdateQueuePositions()
    {
        int index = 0;

        foreach (var npc in queue)
        {
            if (index == 0)
            {
                
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