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

    public GameObject nextStatePrefab;

    public void ChangeState()
    {
        if (nextStatePrefab == null) return;

        Instantiate(nextStatePrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}