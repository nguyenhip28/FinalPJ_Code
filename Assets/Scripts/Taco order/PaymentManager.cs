using UnityEngine;

public class PaymentManager : MonoBehaviour
{
    public TacoOrderUI tacoUI;

    public NPCMovement currentNPC; // 🔥 thêm

    public void Pay()
    {
        int total = tacoUI.CalculateTotal();

        if (total <= 0)
        {
            Debug.Log("No item selected!");
            return;
        }

        PlayerMoney.Instance.Add(total);

        Debug.Log("Paid: " + total);

        // ✅ Ẩn bubble của NPC
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