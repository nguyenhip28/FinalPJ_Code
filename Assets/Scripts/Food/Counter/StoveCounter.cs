using UnityEngine;

public class StoveCounter : BaseCounter
{
    [SerializeField] private CookingProgressUI progressUI;

    public override void Interact(PlayerInteraction player)
    {
        // =========================
        // Nếu bếp chưa có đồ
        // =========================
        if (!HasObject())
        {
            if (player.IsHoldingObject())
            {
                GameObject obj = player.GetHeldObject();
                PlaceObject(obj);
                player.ClearHeld();

                FoodItem food = obj.GetComponent<FoodItem>();
                if (food != null)
                {
                    food.StartCooking();
                    food.OnCookingProgress += progressUI.SetProgress;
                }
            }

            return;
        }

        // =========================
        // Nếu bếp có đồ → lấy xuống
        // =========================
        if (!player.IsHoldingObject())
        {
            GameObject obj = TakeObject();
            player.PickUp(obj);

            FoodItem food = obj.GetComponent<FoodItem>();
            if (food != null)
            {
                food.StopCooking();
                food.OnCookingProgress -= progressUI.SetProgress;
                progressUI.SetProgress(0f);
            }
        }
    }
}