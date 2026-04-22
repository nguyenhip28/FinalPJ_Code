using UnityEngine;

public class Taco : MonoBehaviour
{
    public bool meat;
    public bool lettuce;
    public bool tomato;

    private void OnCollisionEnter(Collision collision)
    {
        NPCMovement npc = collision.collider.GetComponent<NPCMovement>();

        if (npc != null)
        {
            CheckOrder(npc);
        }
    }

    void CheckOrder(NPCMovement npc)
    {
        NPCOrder order = npc.GetCurrentOrder();

        if (order == null) return;

        bool correct =
            order.meat == meat &&
            order.lettuce == lettuce &&
            order.tomato == tomato;

        if (correct)
        {
            npc.OnCorrectOrder();
        }
        else
        {
            npc.OnWrongOrder();
        }

        Destroy(gameObject);
    }
}