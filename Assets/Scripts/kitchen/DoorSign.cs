using UnityEngine;
using TMPro;

public class DoorSign : MonoBehaviour
{
    public DoorInteractable door;

    [Header("Text")]
    public TextMeshPro text; // kéo text vào đây

    public void Interact(Transform player)
    {
        if (door == null) return;

        // 👉 đổi trạng thái cửa
        door.Toggle();

        // 👉 update chữ
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