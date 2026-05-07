using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TacoOrderUI : MonoBehaviour
{
    public static TacoOrderUI Instance;

    public TMP_Text quantityText;
    public TMP_Text totalText;

    public Toggle lettuceToggle;
    public Toggle tomatoToggle;
    public Toggle meatToggle;

    public Button payButton;

    private int quantity = 1;
    private int basePrice = 25;

    private NPCOrder currentOrder;

    void Awake()
    {
        Instance = this;
    }

    public void SetupFromNPC(NPCOrder order)
    {
        currentOrder = order;

        lettuceToggle.isOn = order.lettuce;
        tomatoToggle.isOn = order.tomato;
        meatToggle.isOn = order.meat;

        lettuceToggle.interactable = false;
        tomatoToggle.interactable = false;
        meatToggle.interactable = false;

        payButton.interactable = true;

        UpdateUI();
    }

    public void UpdateUI()
    {
        quantityText.text = quantity.ToString();
        totalText.text = "Total: " + CalculateTotal() + " $";
    }

    public int CalculateTotal()
    {
        int price = basePrice;

        if (lettuceToggle.isOn) price += 5;
        if (tomatoToggle.isOn) price += 5;
        if (meatToggle.isOn) price += 10;

        return price * quantity;
    }

    public void OnClickPayment()
    {
        int total = CalculateTotal();
        PlayerMoney.Instance.Add(total);

        Debug.Log("Paid: " + total);
    }

    public NPCOrder GetOrder()
    {
        return currentOrder;
    }

    public void ResetUI()
    {
        quantity = 1;

        lettuceToggle.isOn = false;
        tomatoToggle.isOn = false;
        meatToggle.isOn = false;

        lettuceToggle.interactable = true;
        tomatoToggle.interactable = true;
        meatToggle.interactable = true;

        payButton.interactable = false; 

        currentOrder = null;

        UpdateUI();
    }

    public TacoData GetOrderData()
    {
        TacoData data = new TacoData();

        data.meat = meatToggle.isOn ? 1 : 0;
        data.lettuce = lettuceToggle.isOn;
        data.tomato = tomatoToggle.isOn;

        return data;
    }
}