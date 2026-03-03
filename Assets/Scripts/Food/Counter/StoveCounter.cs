using UnityEngine;

public class StoveCounter : BaseCounter
{
    public override void Interact(PlayerInteraction player)
    {
        // Nếu bếp chưa có đồ
        if (!HasObject())
        {
            if (player.IsHoldingObject())
            {
                GameObject obj = player.GetHeldObject();  // lấy object trước
                PlaceObject(obj);
                player.ClearHeld();                      // rồi mới clear
            }
        }
        else
        {
            FoodItem food = currentObject.GetComponent<FoodItem>();

            if (food != null)
            {
                food.StartCooking();
            }
        }
    }
}