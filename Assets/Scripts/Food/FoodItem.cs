using UnityEngine;

public enum FoodState
{
    Raw,
    Chopped,
    Cooked
}

public class FoodItem : MonoBehaviour
{
    public string foodName;
    public FoodState currentState;

    [Header("State Prefabs")]
    public GameObject choppedPrefab;
    public GameObject cookedPrefab;

    public void Chop()
    {
        if (currentState != FoodState.Raw || choppedPrefab == null)
            return;

        GameObject newFood = Instantiate(
            choppedPrefab,
            transform.position,
            transform.rotation
        );

        Destroy(gameObject);
    }

    public void Cook()
    {
        if (currentState != FoodState.Chopped || cookedPrefab == null)
            return;

        GameObject newFood = Instantiate(
            cookedPrefab,
            transform.position,
            transform.rotation
        );

        Destroy(gameObject);
    }
}