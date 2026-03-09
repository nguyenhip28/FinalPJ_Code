using UnityEngine;

public class CuttingCounter : BaseCounter
{
    private int cutCount = 0;
    private int requiredCuts = 3;

    public override void Interact(PlayerInteraction player)
    {
        // =========================
        // 1️⃣ Đặt food lên thớt
        // =========================
        if (!HasFood())
        {
            GameObject held = player.GetHeldObject();

            if (held != null)
            {
                FoodItem food = held.GetComponent<FoodItem>();

                if (food != null && !HasFood())
                {
                    PlaceObject(held);
                    player.ClearHeld();
                    cutCount = 0;
                }
            }

            return;
        }

        // =========================
        // 2️⃣ Lấy food xuống
        // =========================
        if (!player.HasKnife())
        {
            if (player.GetHeldObject() == null)
            {
                GameObject food = TakeObject();

                if (food != null)
                {
                    player.PickUp(food);
                    cutCount = 0;
                }
            }

            return;
        }

        // =========================
        // 3️⃣ Cắt food
        // =========================
        FoodItem foodItem = currentFood.GetComponent<FoodItem>();

        if (foodItem == null)
            return;

        cutCount++;

        Debug.Log("Cut progress: " + cutCount + "/" + requiredCuts);

        if (cutCount >= requiredCuts)
        {
            foodItem.Chop();
            cutCount = 0;
        }
    }
}