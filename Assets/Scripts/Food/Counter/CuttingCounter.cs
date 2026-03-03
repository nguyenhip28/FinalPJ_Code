using UnityEngine;

public class CuttingCounter : BaseCounter
{
    private int cutCount = 0;
    private int requiredCuts = 3;

    public override void Interact(PlayerInteraction player)
    {
        // ===== Nếu chưa có object trên bàn =====
        if (!HasObject())
        {
            if (player.IsHoldingObject())
            {
                PlaceObject(player.GetHeldObject());
                player.ClearHeld();
            }
            return;
        }

        // ===== Phải cầm dao mới được cắt =====
        if (!player.HasKnife())
        {
            Debug.Log("Need knife to cut!");
            return;
        }

        // ===== Có object trên bàn =====
        FoodItem food = currentObject.GetComponent<FoodItem>();

        if (food == null)
            return;

        if (food.currentState != FoodState.Raw)
            return;

        cutCount++;

        Debug.Log("Cut progress: " + cutCount + "/" + requiredCuts);

        if (cutCount >= requiredCuts)
        {
            food.Chop();
            currentObject = null;
            cutCount = 0;
        }
    }
}