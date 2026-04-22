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

        // 💰 cộng tiền
        PlayerMoney.Instance.Add(total);

        Debug.Log("Paid: " + total);

        // 👉 báo NPC đã order xong
        if (currentNPC != null)
        {
            currentNPC.CompleteOrder();
        }

        ResetOrder();
    }

    void ResetOrder()
    {
        tacoUI.ResetUI();
    }
}