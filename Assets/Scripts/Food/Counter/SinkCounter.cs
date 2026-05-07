using UnityEngine;

public class SinkCounter : BaseCounter
{
    private float washProgress = 0f;
    private float washTime = 2f;

    public override void Interact(PlayerInteraction player)
    {
        if (HasObject())
        {
            FoodItem food = currentFood.GetComponent<FoodItem>();

            if (food != null && food.currentState == FoodState.Raw)
            {
                washProgress += Time.deltaTime;

                if (washProgress >= washTime)
                {
                    washProgress = 0f;
                    food.currentState = FoodState.Chopped;
                    Debug.Log("Food washed!");
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