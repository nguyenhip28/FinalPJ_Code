using UnityEngine;
using TMPro;

public class ItemUI : MonoBehaviour
{
    public int price = 50;

    public TextMeshProUGUI quantityText;

    private int quantity = 0;

    public void Add()
    {
        quantity++;
        UpdateUI();
    }

    public void Remove()
    {
        if (quantity > 0)
        {
            quantity--;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        quantityText.text = quantity.ToString();
        CartManager.Instance.UpdateTotal();
    }

    public int GetTotalPrice()
    {
        return quantity * price;
    }
    public int GetQuantity()
    {
        return quantity;
    }

    public void ResetItem()
    {
        quantity = 0;
        UpdateUI();
    }
}