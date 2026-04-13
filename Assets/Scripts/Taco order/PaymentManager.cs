using UnityEngine;

public class PaymentManager : MonoBehaviour
{
    public TacoOrderUI tacoUI;

    public void Pay()
    {
        int total = tacoUI.CalculateTotal();

        if (total <= 0)
        {
            Debug.Log("No item selected!");
            return;
        }

        Debug.Log("Customer paid: " + total + "$");

        // Reset order sau khi thanh toán
        ResetOrder();
    }

    void ResetOrder()
    {
        tacoUI.SendMessage("Decrease"); // hoặc reset quantity về 0
    }
}