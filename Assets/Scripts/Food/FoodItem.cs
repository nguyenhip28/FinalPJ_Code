using UnityEngine;

public enum FoodState
{
    Raw,
    Chopped,
    Cooked,
    Burned
}

public class FoodItem : MonoBehaviour
{
    public string foodName;
    public FoodState currentState;

    [Header("State Prefabs")]
    public GameObject choppedPrefab;
    public GameObject cookedPrefab;
    public GameObject burnedPrefab;

    [Header("Cooking Settings")]
    public float cookTime = 5f;
    public float burnTime = 10f;

    private float timer = 0f;
    private bool isCooking = false;

    void Update()
    {
        if (!isCooking) return;

        timer += Time.deltaTime;

        // Chopped → Cooked
        if (currentState == FoodState.Chopped && timer >= cookTime)
        {
            ChangeState(cookedPrefab);
        }

        // Cooked → Burned
        if (currentState == FoodState.Cooked && timer >= burnTime)
        {
            ChangeState(burnedPrefab);
        }
    }

    public void Chop()
    {
        if (currentState != FoodState.Raw || choppedPrefab == null)
            return;

        Instantiate(choppedPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void StartCooking()
    {
        if (currentState == FoodState.Chopped)
        {
            isCooking = true;
            timer = 0f;
        }
    }

    void ChangeState(GameObject prefab)
    {
        if (prefab == null) return;

        Instantiate(prefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}