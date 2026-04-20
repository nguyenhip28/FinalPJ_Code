using UnityEngine;

public class FoodBox : MonoBehaviour
{
    [Header("Food Settings")]
    public GameObject foodPrefab;   // loại food spawn
    public int maxAmount = 10;

    private int currentAmount;
    public int boxID;

    void Start()
    {
        currentAmount = maxAmount;
    }

    // ===== CHECK CÒN FOOD =====
    public bool HasFood()
    {
        return currentAmount > 0;
    }

    // ===== DÙNG 1 ITEM =====
    public void UseOne()
    {
        if (currentAmount <= 0)
        {
            Debug.Log("Box đã hết đồ!");
            return;
        }

        currentAmount--;

        Debug.Log("Còn lại: " + currentAmount);
    }

    // ===== OPTIONAL: CLICK VÀO BOX =====
    // (giữ lại để KHÔNG bị lỗi Interact)
    public void Interact(PlayerInteraction player)
    {
        Debug.Log("Click vào box");
    }

    // ===== OPTIONAL: LẤY SỐ LƯỢNG HIỆN TẠI =====
    public int GetCurrentAmount()
    {
        return currentAmount;
    }

    public BoxData GetData()
    {
        BoxData data = new BoxData();

        data.boxID = boxID;
        data.amount = currentAmount;

        return data;
    }

    public void LoadFromData(BoxData data)
    {
        currentAmount = data.amount;
    }
}