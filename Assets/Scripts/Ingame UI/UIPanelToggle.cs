using UnityEngine;

public class UIPanelToggle : MonoBehaviour
{
    public GameObject panel;
    private bool isOpen = true;

    public void Toggle()
    {
        isOpen = !isOpen;
        panel.SetActive(isOpen);
    }
}