using UnityEngine;

public class FoodBox : BaseCounter
{
    public override void Interact(PlayerInteraction player)
    {
        // Nếu player chưa cầm gì → cầm box
        if (!player.IsHoldingAnything())
        {
            player.PickUp(gameObject);
        }
    }
}