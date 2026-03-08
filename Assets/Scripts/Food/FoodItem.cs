using UnityEngine;
using System;

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
    Cooked,
    Burned
}

public class FoodItem : MonoBehaviour
{
    [Header("Food Info")]
    public FoodType foodType;
    public FoodState currentState = FoodState.Raw;

    [Header("Meshes (For vegetables)")]
    [SerializeField] private Mesh rawMesh;
    [SerializeField] private Mesh choppedMesh;
    [SerializeField] private Mesh cookedMesh;
    [SerializeField] private Mesh burnedMesh;

    [Header("Materials (For meat color change)")]
    [SerializeField] private Material rawMaterial;
    [SerializeField] private Material cookedMaterial;
    [SerializeField] private Material burnedMaterial;

    [Header("Cooking Settings")]
    [SerializeField] private float cookTime = 5f;
    [SerializeField] private float burnTime = 10f;

    [SerializeField] private GameObject cookingBarPrefab;

    private CookingProgressUI progressUI;

    public Action<float> OnCookingProgress;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private float cookTimer = 0f;
    private bool isCooking = false;

    // =====================================================
    // INITIALIZE
    // =====================================================

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        ApplyRawVisual();
    }

    // =====================================================
    // UPDATE (COOK TIMER)
    // =====================================================

    private void Update()
    {
        if (!isCooking) return;

        cookTimer += Time.deltaTime;

        Debug.Log(foodType + " cooking... " + cookTimer);

        float progress = Mathf.Clamp01(cookTimer / burnTime);
        OnCookingProgress?.Invoke(progress);

        // Burn
        if (cookTimer >= burnTime)
        {
            Burn();
            isCooking = false;
            return;
        }

        // Cook
        if (cookTimer >= cookTime && currentState != FoodState.Cooked)
        {
            Cook();
        }
    }

    // =====================================================
    // CHOP
    // =====================================================

    public void Chop()
    {
        if (foodType == FoodType.Meat)
        {
            if (currentState != FoodState.Cooked) return;
        }
        else
        {
            if (currentState != FoodState.Raw) return;
        }

        currentState = FoodState.Chopped;

        if (choppedMesh != null)
            meshFilter.mesh = choppedMesh;

        Debug.Log(foodType + " chopped!");
    }

    // =====================================================
    // START COOKING
    // =====================================================

    public void StartCooking()
    {
        if (currentState == FoodState.Raw || currentState == FoodState.Chopped)
        {
            cookTimer = 0f;
            isCooking = true;

            if (progressUI == null && cookingBarPrefab != null)
            {
                Debug.Log("Cooking bar spawned");

                GameObject bar = Instantiate(cookingBarPrefab);

                bar.transform.SetParent(transform);
                bar.transform.localPosition = new Vector3(0, 0.5f, 0);
                bar.transform.localRotation = Quaternion.identity;
                bar.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                progressUI = bar.GetComponent<CookingProgressUI>();
                progressUI.SetFood(this);
            }
        }
    }

    // =====================================================
    // STOP COOKING
    // =====================================================

    public void StopCooking()
    {
        isCooking = false;
    }

    // =====================================================
    // COOK
    // =====================================================

    private void Cook()
    {
        if (currentState == FoodState.Cooked || currentState == FoodState.Burned)
            return;

        currentState = FoodState.Cooked;

        if (foodType == FoodType.Meat)
        {
            if (cookedMaterial != null)
                meshRenderer.material = cookedMaterial;
        }
        else
        {
            if (cookedMesh != null)
                meshFilter.mesh = cookedMesh;
        }

        Debug.Log(foodType + " cooked!");
    }

    // =====================================================
    // BURN
    // =====================================================

    private void Burn()
    {
        if (currentState != FoodState.Cooked)
            return;

        currentState = FoodState.Burned;

        if (foodType == FoodType.Meat)
        {
            if (burnedMaterial != null)
                meshRenderer.material = burnedMaterial;
        }
        else
        {
            if (burnedMesh != null)
                meshFilter.mesh = burnedMesh;
        }

        if (progressUI != null)
            progressUI.gameObject.SetActive(false);

        Debug.Log(foodType + " burned!");
    }

    // =====================================================
    // VISUAL RESET
    // =====================================================

    private void ApplyRawVisual()
    {
        if (rawMesh != null)
            meshFilter.mesh = rawMesh;

        if (rawMaterial != null)
            meshRenderer.material = rawMaterial;
    }

    void OnDestroy()
    {
        OnCookingProgress = null;
    }
}