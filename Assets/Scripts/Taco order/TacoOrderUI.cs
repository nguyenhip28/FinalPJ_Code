using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TacoOrderUI : MonoBehaviour
{
    public TMP_Text quantityText;
    public TMP_Text totalText;

    public Toggle lettuceToggle;
    public Toggle tomatoToggle;
    public Toggle meatToggle;

    private int quantity = 0;

    private int basePrice = 25;

    public void Increase()
    {
        quantity++;
        UpdateUI();
    }

    public void Decrease()
    {
        if (quantity > 0)
            quantity--;

        UpdateUI();
    }

    public void UpdateUI()
    {
        quantityText.text = quantity.ToString();
        totalText.text = "Total: " + CalculateTotal() + " $";
    }

    public int CalculateTotal()
    {
        int toppingPrice = 0;

        if (lettuceToggle.isOn)
            toppingPrice += 5;

        if (tomatoToggle.isOn)
            toppingPrice += 5;

        if (meatToggle.isOn)
            toppingPrice += 10;

        int pricePerTaco = basePrice + toppingPrice;

        return pricePerTaco * quantity;
    }
}