using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity;
    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        mouseSensitivity = PlayerPrefs.GetFloat("sensitivity", 100f);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}