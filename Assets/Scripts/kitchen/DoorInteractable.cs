using UnityEngine;

public class DoorInteractable : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;

    public float openAngle = 90f;
    public float speed = 3f;

    private bool isOpen = false;

    private Quaternion leftClosed;
    private Quaternion rightClosed;

    private Quaternion leftOpen;
    private Quaternion rightOpen;

    void Start()
    {
        leftClosed = leftDoor.rotation;
        rightClosed = rightDoor.rotation;

        // 👉 mở ngược nhau
        leftOpen = Quaternion.Euler(0, leftDoor.eulerAngles.y - openAngle, 0);
        rightOpen = Quaternion.Euler(0, rightDoor.eulerAngles.y + openAngle, 0);
    }

    void Update()
    {
        Quaternion leftTarget = isOpen ? leftOpen : leftClosed;
        Quaternion rightTarget = isOpen ? rightOpen : rightClosed;

        leftDoor.rotation = Quaternion.Slerp(leftDoor.rotation, leftTarget, Time.deltaTime * speed);
        rightDoor.rotation = Quaternion.Slerp(rightDoor.rotation, rightTarget, Time.deltaTime * speed);
    }

    public void Interact(Transform player, bool isSignOpen)
    {
        if (isSignOpen)
        {
            isOpen = true;
        }
        else
        {
            isOpen = false;
        }
    }
}