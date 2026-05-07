using UnityEngine;
using SojaExiles;

public class DoorSwitch : MonoBehaviour
{
    public opencloseDoor doorLeft;
    public opencloseDoor1 doorRight;


    public DoorInteractable doorState;

    public void ToggleBothDoors()
    {
        if (doorLeft != null) doorLeft.ToggleDoor();
        if (doorRight != null) doorRight.ToggleDoor();

        if (doorState != null)
        {
            doorState.Toggle();
        }
    }
}