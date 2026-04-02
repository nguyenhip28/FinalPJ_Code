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

    [Header("Prefab Swap (For meat / special)")]
    [SerializeField] private GameObject choppedPrefab;

    [Header("Materials (For meat color change)")]
    [SerializeField] private Material rawMaterial;
    [SerializeField] private Material cookedMaterial;
    [SerializeField] private Material burnedMaterial;

    [Header("Cooking Settings")]
    [SerializeField] private float cookTime = 5f;
    [SerializeField] private float burnTime = 10f;

    [Header("UI")]
    [SerializeField] private GameObject cookingBarPrefab;

    private CookingBar3D progressUI;

    public Action<float> OnCookingProgress;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private float cookTimer = 0f;
    private bool isCooking = false;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        ApplyRawVisual();
    }

    private void Update()
    {
        if (!isCooking) return;

        cookTimer += Time.deltaTime;

        float progress = Mathf.Clamp01(cookTimer / burnTime);
        OnCookingProgress?.Invoke(progress);

        if (cookTimer >= burnTime)
        {
            Burn();
            isCooking = false;
            return;
        }

        if (cookTimer >= cookTime && currentState != FoodState.Cooked)
        {
            Cook();
        }
    }

    // ===================== CHOP =====================

    public bool CanBeChopped()
    {
        if (foodType == FoodType.Meat)
            return currentState == FoodState.Cooked
                || currentState == FoodState.Burned;

        return currentState == FoodState.Raw;
    }

    public bool CanBePlacedOnCuttingBoard()
    {
        if (foodType == FoodType.Meat)
            return currentState == FoodState.Cooked;

        return currentState == FoodState.Raw;
    }

    public GameObject ChopAndReturnNew()
    {
        if (!CanBeChopped()) return null;

        currentState = FoodState.Chopped;

        // 👉 Ưu tiên prefab (meat)
        if (choppedPrefab != null)
        {
            GameObject newObj = Instantiate(
                choppedPrefab,
                transform.position,
                transform.rotation
            );

            newObj.transform.SetParent(transform.parent);

            Destroy(gameObject);
            return newObj;
        }

        // 👉 fallback (vegetable)
        if (choppedMesh != null)
            meshFilter.mesh = choppedMesh;

        return null;
    }

    // ===================== COOK =====================

    public void StartCooking()
    {
        if (currentState != FoodState.Raw && currentState != FoodState.Chopped)
            return;

        cookTimer = 0f;
        isCooking = true;

        if (progressUI == null && cookingBarPrefab != null)
        {
            GameObject bar = Instantiate(
                cookingBarPrefab,
                transform.position + Vector3.up * 0.4f,
                Quaternion.identity,
                transform
            );

            progressUI = bar.GetComponent<CookingBar3D>();

            if (progressUI != null)
                progressUI.SetFood(this);
        }
    }

    public void StopCooking()
    {
        isCooking = false;

        if (progressUI != null)
            progressUI.Hide();
    }

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
    }

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
    }

    private void ApplyRawVisual()
    {
        if (rawMesh != null)
            meshFilter.mesh = rawMesh;

        if (rawMaterial != null)
            meshRenderer.material = rawMaterial;
    }

    private void OnDestroy()
    {
        OnCookingProgress = null;
    }
}