using UnityEngine;

public class ServingTray : BaseCounter
{
    private TacoData currentOrder;

    // =====================================================
    // NHẬN ORDER TỪ PAYMENT
    // =====================================================
    public void SetOrder(TacoData data)
    {
        currentOrder = data;

        Debug.Log($"📥 Tray nhận order: Meat={data.meat}, Lettuce={data.lettuce}, Tomato={data.tomato}");
    }

    private PaymentManager paymentManager;

    void Start()
    {
        paymentManager = FindObjectOfType<PaymentManager>();
    }

    // =====================================================
    // PLAYER NHẤN E
    // =====================================================
    public override void Interact(PlayerInteraction player)
    {
        if (!player.IsHoldingObject())
        {
            Debug.Log("Tray: Không có gì để đặt");
            return;
        }

        GameObject obj = player.GetHeldObject();

        bool placed = PlaceFood(obj);

        // 🔥 chỉ clear tay nếu đặt thành công
        if (placed)
        {
            player.ClearHeld();
        }
    }

    // =====================================================
    // XỬ LÝ ĐẶT MÓN
    // =====================================================
    private bool PlaceFood(GameObject obj)
    {
        Debug.Log("🍽️ Placed on tray");

        TacoItem taco = obj.GetComponent<TacoItem>();

        if (taco == null)
        {
            Debug.Log("❌ Không phải taco");
            return false;
        }

        if (currentOrder == null)
        {
            Debug.Log("❌ Chưa có order");
            return false;
        }

        Debug.Log($"👉 Player Taco: Meat={taco.data.meat}, Lettuce={taco.data.lettuce}, Tomato={taco.data.tomato}");

        if (taco.data.Compare(currentOrder))
        {
            Debug.Log("✅ ĐÚNG MÓN!");

            if (paymentManager != null && paymentManager.currentNPC != null)
            {
                paymentManager.currentNPC.OnCorrectOrder();
            }

            // 🔥 XÓA TACO (biến mất)
            Destroy(obj);
        }
        else
        {
            Debug.Log("❌ SAI MÓN!");

            if (paymentManager != null && paymentManager.currentNPC != null)
            {
                paymentManager.currentNPC.OnWrongOrder();
            }

            // ❗ có thể vẫn destroy hoặc giữ lại tùy bạn
            Destroy(obj);
        }

        // =====================================================
        // 🔥 ĐẶT TACO LÊN ĐÚNG FOOD PLACE POINT
        // =====================================================
        if (foodPlacePoint == null)
        {
            Debug.LogError("❌ Chưa gán Food Place Point!");
            return false;
        }

        obj.transform.SetParent(foodPlacePoint);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;

        // tắt physics để không rơi
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // reset order
        currentOrder = null;

        return true;
    }
}