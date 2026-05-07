using UnityEngine;

public class PlateCounter : BaseCounter
{
    [Header("Spawn Point")]
    [SerializeField] private Transform holdPoint;

    [Header("Prefabs")]
    [SerializeField] private GameObject tortillaPrefab;
    [SerializeField] private GameObject saladPrefab;
    [SerializeField] private GameObject tacoPrefab;

    private GameObject currentVisual;

    private TaskManager taskManager;

    void Start()
    {
        taskManager = UnityEngine.Object.FindFirstObjectByType<TaskManager>();      
    }
    private enum PlateState
    {
        Empty,
        HasTortilla,
        HasSalad,
        Complete
    }

    private PlateState state = PlateState.Empty;

    private bool hasTomato = false;
    private bool hasLettuce = false;

    private int currentToppingCount = 0;
    private const int totalTopping = 2;

    public override void Interact(PlayerInteraction player)
    {
        if (!player.IsHoldingObject())
        {
            if (state == PlateState.Complete && currentVisual != null)
            {
                player.PickUp(currentVisual);
                currentVisual = null;

                Debug.Log("🎉 Taco picked up!");

                ResetPlate();
            }
            return;
        }

        GameObject held = player.GetHeldObject();
        FoodItem food = held.GetComponent<FoodItem>();

        if (food == null) return;

        if (TryAddIngredient(food))
        {
            Destroy(held);
            player.ClearHeld();
        }
    }

    private bool TryAddIngredient(FoodItem food)
    {
        switch (state)
        {
            case PlateState.Empty:
                if (food.foodType == FoodType.Tortilla)
                {
                    Debug.Log("🌮 Add Tortilla");

                    SetState(PlateState.HasTortilla);

                    if (taskManager != null)
                        taskManager.AddTacoStep();

                    return true;
                }
                break;

            case PlateState.HasTortilla:

                if (food.foodType == FoodType.Tomato &&
                    food.currentState == FoodState.Chopped &&
                    !hasTomato)
                {
                    hasTomato = true;
                    currentToppingCount++;

                    if (taskManager != null)
                        taskManager.AddTacoStep();

                    Debug.Log($"🌮 Add Tomato ({currentToppingCount}/{totalTopping})");

                    CheckVegComplete();
                    return true;
                }

                if (food.foodType == FoodType.Lettuce &&
                    food.currentState == FoodState.Chopped &&
                    !hasLettuce)
                {
                    hasLettuce = true;
                    currentToppingCount++;

                    if (taskManager != null)
                        taskManager.AddTacoStep();

                    Debug.Log($"🌮 Add Lettuce ({currentToppingCount}/{totalTopping})");

                    CheckVegComplete();
                    return true;
                }

                break;

            case PlateState.HasSalad:
                if (food.foodType == FoodType.Meat &&
                    food.currentState == FoodState.Chopped)
                {
                    Debug.Log("🔥 Add Meat → COMPLETE!");

                    SetState(PlateState.Complete);

                    if (taskManager != null)
                        taskManager.AddTacoStep();


                    return true;
                }
                break;
        }

        return false;
    }

    private void CheckVegComplete()
    {
        if (hasTomato && hasLettuce)
        {
            Debug.Log("✅ Veg complete! Ready for meat!");

            SetState(PlateState.HasSalad);
        }
    }

    private void SetState(PlateState newState)
    {
        state = newState;

        if (currentVisual != null)
        {
            Destroy(currentVisual);
            currentVisual = null;
        }

        switch (state)
        {
            case PlateState.HasTortilla:
                currentVisual = Instantiate(tortillaPrefab, holdPoint);
                break;

            case PlateState.HasSalad:
                currentVisual = Instantiate(saladPrefab, holdPoint);
                break;

            case PlateState.Complete:
                currentVisual = Instantiate(tacoPrefab, holdPoint);
                TacoItem tacoItem = currentVisual.GetComponent<TacoItem>();

                if (tacoItem != null)
                {
                    tacoItem.data = new TacoData()
                    {
                        meat = 1, 
                        lettuce = hasLettuce,
                        tomato = hasTomato
                    };
                }
                break;
        }

        if (currentVisual != null)
        {
            currentVisual.transform.localPosition = Vector3.zero;
            currentVisual.transform.localRotation = Quaternion.identity;
        }
    }

    private void ResetPlate()
    {
        hasTomato = false;
        hasLettuce = false;
        currentToppingCount = 0;
        state = PlateState.Empty;
    }
}