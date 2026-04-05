using UnityEngine;
using TMPro;

public class CartManager : MonoBehaviour
{
    public static CartManager Instance;

    public ItemUI[] items;

    public TextMeshProUGUI totalText;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateTotal()
    {
        int total = 0;

        foreach (ItemUI item in items)
        {
            total += item.GetTotalPrice();
        }

        totalText.text = "Total: " + total.ToString();
    }
}