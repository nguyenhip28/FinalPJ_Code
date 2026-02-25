using UnityEngine;

public class Stove : MonoBehaviour
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

        Rigidbody rb = food.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        FoodItem item = food.GetComponent<FoodItem>();
        if (item != null)
        {
            item.StartCooking();
        }
    }

    public GameObject TakeFood()
    {
        if (currentFood == null) return null;

        GameObject food = currentFood;
        currentFood = null;

        food.transform.SetParent(null);

        Rigidbody rb = food.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        return food;
    }
}