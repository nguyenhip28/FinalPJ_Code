using UnityEngine;

public class CuttingCounter : BaseCounter
{
    private int cutCount = 0;
    private int requiredCuts = 3;

    public override void Interact(PlayerInteraction player)
    {
        // =========================
        // 1️⃣ Nếu chưa có food trên board
        // =========================
        if (!HasFood())
        {
            if (player.IsHoldingObject())
            {
                GameObject held = player.GetHeldObject();

                FoodItem food = held.GetComponent<FoodItem>();
                if (food != null)
                {
                    PlaceObject(held);
                    player.ClearHeld();
                }
            }
            return;
        }

        // =========================
        // 2️⃣ Nếu có food và player KHÔNG cầm dao → lấy food xuống
        // =========================
        if (HasFood() && !player.HasKnife())
        {
            if (!player.IsHoldingObject())
            {
                GameObject food = TakeObject();
                player.PickUp(food);
                cutCount = 0;
            }
            return;
        }

        // =========================
        // 3️⃣ Nếu có food và player có dao → cắt
        // =========================
        if (HasFood() && player.HasKnife())
        {
            FoodItem food = currentFood.GetComponent<FoodItem>();

            if (food == null)
                return;

            if (food.currentState != FoodState.Raw)
                return;

            cutCount++;

            Debug.Log("Cut progress: " + cutCount + "/" + requiredCuts);

            if (cutCount >= requiredCuts)
            {
                food.Chop();
                cutCount = 0;
            }
        }
    }
}