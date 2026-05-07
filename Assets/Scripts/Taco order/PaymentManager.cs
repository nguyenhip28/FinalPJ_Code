using UnityEngine;

public class PaymentManager : MonoBehaviour
{
    public TacoOrderUI tacoUI;
    public NPCMovement currentNPC;
    public ServingTray tray;
    public TimeManager timeManager;

    public void Pay()
    {
        int total = tacoUI.CalculateTotal();

        if (total <= 0)
        {
            Debug.Log("No item selected!");
            return;
        }

        PlayerMoney.Instance.Add(total);

        TacoData orderData = tacoUI.GetOrderData();

        
        int tacoCount = 1;

        
        if (timeManager != null)
        {
            timeManager.AddSale(tacoCount, total);
        }

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