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
        
        meatIcon?.SetActive(true);
        lettuceIcon?.SetActive(true);
        tomatoIcon?.SetActive(true);

        
        meatCheck?.SetActive(order != null && order.meat);
        lettuceCheck?.SetActive(order != null && order.lettuce);
        tomatoCheck?.SetActive(order != null && order.tomato);
    }

    void LateUpdate()
    {
        Camera cam = Camera.main;
        if (cam == null) return; 
        transform.forward = cam.transform.forward;
    }
}