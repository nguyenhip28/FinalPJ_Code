using UnityEngine;

public enum FoodType
{
    Tomato,
    Lettuce,
    Meat,
    Tortilla
}

public enum FoodState
{
    Raw,
    Chopped,
    Cooking,
    Cooked,
    Burned
}

public class FoodItem : MonoBehaviour
{
    [Header("Food Info")]
    public FoodType foodType;
    public FoodState currentState;

    [Header("Prefabs")]
    public GameObject choppedPrefab;
    public GameObject cookedPrefab;
    public GameObject burnedPrefab;

    [Header("Cooking")]
    public float cookTime = 5f;
    public float burnTime = 8f;

    private float timer;
    private bool isCooking;

    void Update()
    {
        if (!isCooking) return;

        timer += Time.deltaTime;

        if (currentState == FoodState.Cooking && timer >= cookTime)
        {
            ReplaceWith(cookedPrefab, FoodState.Cooked);
        }

        if (currentState == FoodState.Cooked && timer >= burnTime)
        {
            ReplaceWith(burnedPrefab, FoodState.Burned);
        }
    }

    public void Chop()
    {
        if (currentState != FoodState.Raw) return;

        ReplaceWith(choppedPrefab, FoodState.Chopped);
    }

    public void StartCooking()
    {
        if (currentState == FoodState.Chopped)
        {
            currentState = FoodState.Cooking;
            isCooking = true;
            timer = 0f;
        }
    }

    void ReplaceWith(GameObject prefab, FoodState newState)
    {
        if (prefab == null) return;

        GameObject newObj = Instantiate(prefab, transform.position, transform.rotation);
        FoodItem newFood = newObj.GetComponent<FoodItem>();

        if (newFood != null)
        {
            newFood.currentState = newState;
        }

        Destroy(gameObject);
    }
}