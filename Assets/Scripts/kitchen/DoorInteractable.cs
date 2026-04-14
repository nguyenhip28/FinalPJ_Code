using UnityEngine;

public class DoorInteractable : MonoBehaviour
{
    [Header("Door Parts")]
    public Transform leftDoor;
    public Transform rightDoor;

    [Header("Settings")]
    public float openAngle = 90f;
    public float speed = 3f;

    private bool isOpen = false;

    private Quaternion leftClosed;
    private Quaternion rightClosed;

    private Quaternion leftOpen;
    private Quaternion rightOpen;

    void Start()
    {
        // Lấy rotation ban đầu
        leftClosed = leftDoor.localRotation;
        rightClosed = rightDoor.localRotation;

        // Tính góc mở
        leftOpen = leftClosed * Quaternion.Euler(0, -openAngle, 0);
        rightOpen = rightClosed * Quaternion.Euler(0, openAngle, 0);
    }

    void Update()
    {
        Quaternion leftTarget = isOpen ? leftOpen : leftClosed;
        Quaternion rightTarget = isOpen ? rightOpen : rightClosed;

        leftDoor.localRotation = Quaternion.Slerp(
            leftDoor.localRotation,
            leftTarget,
            Time.deltaTime * speed
        );

        rightDoor.localRotation = Quaternion.Slerp(
            rightDoor.localRotation,
            rightTarget,
            Time.deltaTime * speed
        );
    }

    // 👉 Toggle cửa (QUAN TRỌNG)
    public void Toggle()
    {
        isOpen = !isOpen;
    }

    // 👉 Cho script khác biết trạng thái
    public bool IsOpen()
    {
        return isOpen;
    }
}