using UnityEngine;

public class ServingTray : BaseCounter
{
    private TacoData currentOrder;

    private PaymentManager paymentManager;
    private NPCQueueManager queueManager;

    void Start()
    {
        paymentManager = FindObjectOfType<PaymentManager>();
        queueManager = FindObjectOfType<NPCQueueManager>(); 
    }

    public void SetOrder(TacoData data)
    {
        currentOrder = data;

        Debug.Log($"📥 Tray nhận order: Meat={data.meat}, Lettuce={data.lettuce}, Tomato={data.tomato}");
    }

    public override void Interact(PlayerInteraction player)
    {
        if (!player.IsHoldingObject())
        {
            Debug.Log("Tray: Không có gì để đặt");
            return;
        }

        GameObject obj = player.GetHeldObject();

        bool placed = PlaceFood(obj);

        if (placed)
        {
            player.ClearHeld();
        }
    }

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

        NPCMovement firstNPC = null;

        if (queueManager != null)
        {
            firstNPC = queueManager.GetFirstNPC();
        }

        if (taco.data.Compare(currentOrder))
        {
            Debug.Log("✅ ĐÚNG MÓN!");

            if (firstNPC != null)
            {
                firstNPC.OnCorrectOrder();
            }

            Destroy(obj);
        }
        else
        {
            Debug.Log("❌ SAI MÓN!");

            if (firstNPC != null)
            {
                firstNPC.OnWrongOrder();
            }

            Destroy(obj);
        }

        if (foodPlacePoint == null)
        {
            Debug.LogError("❌ Chưa gán Food Place Point!");
            return false;
        }

        obj.transform.SetParent(foodPlacePoint);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        currentOrder = null;

        return true;
    }
}