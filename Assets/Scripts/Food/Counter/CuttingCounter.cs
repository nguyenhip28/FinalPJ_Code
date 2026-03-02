using UnityEngine;

public class CuttingCounter : BaseCounter
{
    private int cutCount = 0;
    private int requiredCuts = 3;

    public void Interact(PlayerInteraction player)
    {
        if (HasObject())
        {
            FoodItem food = currentObject.GetComponent<FoodItem>();

            if (food != null && food.currentState == FoodState.Raw)
            {
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
        else
        {
            if (player.IsHoldingObject())
            {
                PlaceObject(player.GetHeldObject());
                player.ClearHeld();
            }
        }
    }
}