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

        if (choppedMesh != null)
            meshFilter.mesh = choppedMesh;

        return null;
    }


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
                transform.position + Vector3.up * 0.2f,
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

    public FoodSaveData GetData()
    {
        FoodSaveData data = new FoodSaveData();

        data.foodType = (int)foodType;
        data.state = (int)currentState;

        data.posX = transform.position.x;
        data.posY = transform.position.y;
        data.posZ = transform.position.z;

        data.rotY = transform.eulerAngles.y;

        data.isCooking = isCooking;
        data.cookTimer = cookTimer;

        return data;
    }

    public void LoadFromData(FoodSaveData data)
    {
        foodType = (FoodType)data.foodType;
        currentState = (FoodState)data.state;

        transform.position = new Vector3(data.posX, data.posY, data.posZ);
        transform.rotation = Quaternion.Euler(0, data.rotY, 0);

        cookTimer = data.cookTimer;
        isCooking = data.isCooking;

        ApplyStateVisual();
    }

    void ApplyStateVisual()
    {
        switch (currentState)
        {
            case FoodState.Raw:
                ApplyRawVisual();
                break;

            case FoodState.Chopped:
                if (choppedMesh != null)
                    meshFilter.mesh = choppedMesh;
                break;

            case FoodState.Cooked:
                Cook();
                break;

            case FoodState.Burned:
                Burn();
                break;
        }
    }
}