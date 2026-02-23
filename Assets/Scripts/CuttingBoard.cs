using UnityEngine;

public class CuttingBoard : MonoBehaviour
{
    public Transform placePoint;
    public GameObject currentFood;

    public bool HasFood()
    {
        return currentFood != null;
    }

    public void PlaceFood(GameObject food)
    {
        currentFood = food;
        food.transform.position = placePoint.position;
        food.transform.rotation = placePoint.rotation;
    }

    public void RemoveFood()
    {
        currentFood = null;
    }
}