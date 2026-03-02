using UnityEngine;
using System.Collections.Generic;

public class PlateCounter : BaseCounter
{
    public GameObject tacoPrefab;

    private List<FoodType> ingredients = new List<FoodType>();

    public void Interact(PlayerInteraction player)
    {
        // Nếu player đang cầm food
        if (player.IsHoldingObject())
        {
            FoodItem food = player.GetHeldObject().GetComponent<FoodItem>();

            if (food != null)
            {
                if (food.currentState == FoodState.Chopped ||
                    food.currentState == FoodState.Cooked)
                {
                    ingredients.Add(food.foodType);
                    Destroy(food.gameObject);
                    player.ClearHeld();

                    CheckRecipe();
                }
            }
        }
    }

    void CheckRecipe()
    {
        if (ingredients.Contains(FoodType.Meat) &&
            ingredients.Contains(FoodType.Tomato) &&
            ingredients.Contains(FoodType.Lettuce) &&
            ingredients.Contains(FoodType.Tortilla))
        {
            Debug.Log("🌮 TACO COMPLETED!");

            if (tacoPrefab != null)
            {
                Instantiate(tacoPrefab, transform.position + Vector3.up, Quaternion.identity);
            }

            ingredients.Clear();
        }
    }
}