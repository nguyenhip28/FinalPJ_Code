using UnityEngine;

public class SinkCounter : BaseCounter
{
    private float washProgress = 0f;
    private float washTime = 2f;

    public override void Interact(PlayerInteraction player)
    {
        // Nếu có đồ trên bồn
        if (HasObject())
        {
            FoodItem food = currentObject.GetComponent<FoodItem>();

            if (food != null && food.currentState == FoodState.Raw)
            {
                washProgress += Time.deltaTime;

                if (washProgress >= washTime)
                {
                    washProgress = 0f;
                    food.currentState = FoodState.Chopped;
                    // Nếu m có trạng thái Washed riêng thì đổi lại thành Washed
                    Debug.Log("Food washed!");
                }
            }
        }
        else
        {
            // Nếu bồn trống và player đang cầm đồ
            if (player.IsHoldingObject())
            {
                PlaceObject(player.GetHeldObject());
                player.ClearHeld();
            }
        }
    }
}