using UnityEngine;

public class CuttingBoard : MonoBehaviour
{
    private FoodItem currentFood;

    private void OnTriggerEnter(Collider other)
    {
        FoodItem food = other.GetComponent<FoodItem>();

        if (food != null)
        {
            currentFood = food;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        FoodItem food = other.GetComponent<FoodItem>();

        if (food != null && food == currentFood)
        {
            currentFood = null;
        }
    }

    private void Update()
    {
        if (currentFood != null && Input.GetKeyDown(KeyCode.F))
        {
            currentFood.ChangeState();
            currentFood = null;
        }
    }
}