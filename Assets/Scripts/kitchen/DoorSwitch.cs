using UnityEngine;
using SojaExiles;

public class DoorSwitch : MonoBehaviour
{
    public opencloseDoor doorLeft;
    public opencloseDoor1 doorRight;

    // 🔥 THÊM DÒNG NÀY
    public DoorInteractable doorState;

    public void ToggleBothDoors()
    {
        if (doorLeft != null) doorLeft.ToggleDoor();
        if (doorRight != null) doorRight.ToggleDoor();

        // 🔥 QUAN TRỌNG: cập nhật trạng thái cho NPC
        if (doorState != null)
        {
            doorState.Toggle();
        }
    }
}