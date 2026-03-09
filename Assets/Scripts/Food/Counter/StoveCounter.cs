using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter
{
    [Header("Meat Slots")]
    [SerializeField] private List<Transform> meatPoints;

    [Header("UI")]
    [SerializeField] private CookingProgressUI progressUI;

    private List<FoodItem> cookingFoods = new List<FoodItem>();


    // =====================================================
    // INTERACT
    // =====================================================

    public override void Interact(PlayerInteraction player)
    {
        Debug.Log("Stove Interact Called");

        // =====================================
        // CASE 1: Player không cầm gì → lấy thịt
        // =====================================
        if (!player.IsHoldingObject())
        {
            if (cookingFoods.Count > 0)
            {
                FoodItem food = cookingFoods[0];

                GameObject meat = TakeMeat(food);

                if (meat != null)
                {
                    player.PickUp(meat);
                }
            }

            return;
        }

        // =====================================
        // CASE 2: Player đang cầm → đặt lên bếp
        // =====================================
        GameObject obj = player.GetHeldObject();
        FoodItem foodItem = obj.GetComponent<FoodItem>();

        if (foodItem == null)
            return;

        if (foodItem.foodType != FoodType.Meat)
            return;

        Transform freeSlot = GetFreeSlot();

        if (freeSlot == null)
        {
            Debug.Log("Stove full!");
            return;
        }

        // đặt thịt lên bếp
        PlaceMeat(foodItem, freeSlot);

        // bỏ khỏi tay player
        player.ClearHeld();

        // bắt đầu nấu
        foodItem.StartCooking();

        if (progressUI != null)
        {
            progressUI.SetFood(foodItem);
        }

        cookingFoods.Add(foodItem);
    }


    // =====================================================
    // FIND EMPTY SLOT
    // =====================================================

    private Transform GetFreeSlot()
    {
        foreach (Transform slot in meatPoints)
        {
            if (slot.childCount == 0)
            {
                return slot;
            }
        }

        return null;
    }


    // =====================================================
    // PLACE MEAT
    // =====================================================

    private void PlaceMeat(FoodItem food, Transform slot)
    {
        Rigidbody rb = food.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // bỏ parent cũ
        food.transform.SetParent(null);

        // reset scale
        food.transform.localScale = Vector3.one;

        // đặt vào vị trí slot
        food.transform.position = slot.position;
        food.transform.rotation = slot.rotation;

        // parent vào slot
        food.transform.SetParent(slot, true);
    }


    // =====================================================
    // TAKE MEAT BACK
    // =====================================================

    public GameObject TakeMeat(FoodItem food)
    {
        if (food == null)
            return null;

        // dừng nấu
        food.StopCooking();

        // bỏ parent khỏi slot
        food.transform.SetParent(null);

        // reset transform để tránh lỗi scale/rotation
        food.transform.localScale = Vector3.one;
        food.transform.rotation = Quaternion.identity;

        // bật physics lại
        Rigidbody rb = food.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // remove khỏi list đang nấu
        cookingFoods.Remove(food);

        return food.gameObject;
    }
}