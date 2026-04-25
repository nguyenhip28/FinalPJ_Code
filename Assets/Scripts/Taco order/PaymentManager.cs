using UnityEngine;

public class PaymentManager : MonoBehaviour
{
    public TacoOrderUI tacoUI;

    public NPCMovement currentNPC; // 🔥 thêm

    public ServingTray tray;

    public void Pay()
    {
        int total = tacoUI.CalculateTotal();

        if (total <= 0)
        {
            Debug.Log("No item selected!");
            return;
        }

        PlayerMoney.Instance.Add(total);

        // 🔥 LẤY DATA TỪ UI
        TacoData orderData = tacoUI.GetOrderData();

        // 🔥 GỬI SANG TRAY
        if (tray != null)
        {
            tray.SetOrder(orderData);
        }

        if (currentNPC != null)
        {
            currentNPC.HideOrderBubble();
        }

        ResetOrder();
    }

    void ResetOrder()
    {
        tacoUI.ResetUI();
    }
}