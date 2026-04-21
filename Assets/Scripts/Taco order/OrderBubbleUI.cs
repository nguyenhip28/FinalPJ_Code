using UnityEngine;
using UnityEngine.UI;

public class OrderBubbleUI : MonoBehaviour
{
    [Header("Icons")]
    public GameObject meatIcon;
    public GameObject lettuceIcon;
    public GameObject tomatoIcon;

    [Header("Checks")]
    public GameObject meatCheck;
    public GameObject lettuceCheck;
    public GameObject tomatoCheck;

    public void Setup(NPCOrder order)
    {
        // HIỆN ICON LUÔN
        meatIcon?.SetActive(true);
        lettuceIcon?.SetActive(true);
        tomatoIcon?.SetActive(true);

        // CHECKMARK nếu có chọn
        meatCheck?.SetActive(order != null && order.meat);
        lettuceCheck?.SetActive(order != null && order.lettuce);
        tomatoCheck?.SetActive(order != null && order.tomato);
    }

    void LateUpdate()
    {
        Camera cam = Camera.main;
        if (cam == null) return; // Guard against missing MainCamera
        transform.forward = cam.transform.forward;
    }
}