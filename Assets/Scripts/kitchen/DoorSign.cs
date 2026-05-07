using UnityEngine;
using TMPro;

public class DoorSign : MonoBehaviour
{
    public DoorInteractable door;

    [Header("Text")]
    public TextMeshPro text; 

    public void Interact(Transform player)
    {
        if (door == null) return;


        door.Toggle();


        UpdateSign();
    }

    void Start()
    {
        UpdateSign();
    }

    void UpdateSign()
    {
        if (text != null)
        {
            text.text = door.IsOpen() ? "OPEN" : "CLOSE";
        }
    }
}