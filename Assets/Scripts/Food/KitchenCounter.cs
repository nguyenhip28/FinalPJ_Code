using UnityEngine;

public class KitchenCounter : MonoBehaviour
{
    public Transform placePoint;
    private GameObject currentFood;

    public bool HasFood()
    {
        return currentFood != null;
    }

    public void PlaceFood(GameObject food)
    {
        currentFood = food;

        food.transform.position = placePoint.position;
        food.transform.rotation = placePoint.rotation;
        food.transform.SetParent(transform);
    }

    public void Chop()
    {
        if (currentFood == null) return;

        FoodItem food = currentFood.GetComponent<FoodItem>();

        if (food != null)
        {
            food.Chop();
        }
    }
}