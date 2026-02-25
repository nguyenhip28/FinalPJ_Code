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

        // Đặt đúng vị trí
        food.transform.position = placePoint.position;
        food.transform.rotation = placePoint.rotation;
        food.transform.SetParent(transform);

        // Tắt physics khi nằm trên bàn
        Rigidbody rb = food.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    public GameObject TakeFood()
    {
        if (currentFood == null) return null;

        GameObject food = currentFood;
        currentFood = null;

        food.transform.SetParent(null);

        // Bật lại physics khi nhấc lên
        Rigidbody rb = food.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        return food;
    }

    public void Chop()
    {
        if (currentFood == null) return;

        FoodItem food = currentFood.GetComponent<FoodItem>();

        if (food != null)
        {
            food.Chop();

            // Sau khi Chop, object cũ bị Destroy
            // Nên cần cập nhật lại currentFood
            currentFood = null;
        }
    }
}