using UnityEngine;

public class DoorSign : MonoBehaviour
{
    public bool isOpenSign = true; // true = OPEN, false = CLOSE
    public DoorInteractable door;

    public void Interact(Transform player)
    {
        door.Interact(player, isOpenSign);
    }
}