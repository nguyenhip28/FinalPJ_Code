using UnityEngine;

public class ComputerInteractionAdvanced : MonoBehaviour
{
    [Header("Camera")]
    public Camera playerCamera;
    public Camera computerCamera;

    [Header("UI")]
    public GameObject computerUI;

    [Header("Player Scripts")]
    public MonoBehaviour playerMovement;
    public MonoBehaviour playerLook;

    public bool isUsingComputer = false;

    public void TryInteract()
    {
        if (isUsingComputer)
        {
            ExitComputer();
        }
        else
        {
            EnterComputer();
        }
    }

    void EnterComputer()
    {
        Debug.Log("ENTER COMPUTER");

        isUsingComputer = true;

        playerCamera.gameObject.SetActive(false);   // 🔥 QUAN TRỌNG
        computerCamera.gameObject.SetActive(true);

        computerUI.SetActive(true);

        playerMovement.enabled = false;
        playerLook.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ExitComputer()
    {
        isUsingComputer = false;

        playerCamera.gameObject.SetActive(true);
        computerCamera.gameObject.SetActive(false);

        computerUI.SetActive(false);

        playerMovement.enabled = true;
        playerLook.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}