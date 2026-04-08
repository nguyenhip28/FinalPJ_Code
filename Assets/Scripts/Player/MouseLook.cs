using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    public void UpdateSensitivity(float value)
    {
        mouseSensitivity = value;
    }

    // 🔥 THÊM
    public float GetXRotation()
    {
        return xRotation;
    }

    // 🔥 THÊM
    public void SetRotation(float xRot)
    {
        xRotation = xRot;
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}