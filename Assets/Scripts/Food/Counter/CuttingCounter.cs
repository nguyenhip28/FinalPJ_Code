using UnityEngine;

public class CuttingCounter : BaseCounter
{
    private int cutCount = 0;
    private int requiredCuts = 3;

    public override void Interact(PlayerInteraction player)
    {
        if (!HasFood())
        {
            GameObject held = player.GetHeldObject();

            if (held != null)
            {
                FoodItem food = held.GetComponent<FoodItem>();

                if (food != null)
                {
                    if (!food.CanBePlacedOnCuttingBoard())
                    {
                        Debug.Log("Không thể đặt lên thớt!");
                        return;
                    }

                    PlaceObject(held);
                    player.ClearHeld();
                    cutCount = 0;
                }
            }

            return;
        }

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

        FoodItem foodItem = currentFood.GetComponent<FoodItem>();

        if (foodItem == null)
            return;

        if (!foodItem.CanBeChopped())
        {
            Debug.Log("Không thể cắt!");
            return;
        }

        cutCount++;

        Debug.Log("Cut progress: " + cutCount + "/" + requiredCuts);

        if (cutCount >= requiredCuts)
        {
            GameObject newFood = foodItem.ChopAndReturnNew();

            if (newFood != null)
            {
                currentFood = newFood;
            }

            cutCount = 0;
        }
    }
}