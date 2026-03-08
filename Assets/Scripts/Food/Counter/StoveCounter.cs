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

        if (!player.IsHoldingObject())
            return;

        GameObject obj = player.GetHeldObject();
        FoodItem food = obj.GetComponent<FoodItem>();

        if (food == null)
            return;

        if (food.foodType != FoodType.Meat)
            return;

        Transform freeSlot = GetFreeSlot();

        if (freeSlot == null)
        {
            Debug.Log("Stove full!");
            return;
        }

        // đặt thịt lên bếp
        PlaceMeat(food, freeSlot);

        // bỏ khỏi tay player
        player.ClearHeld();

        // bắt đầu nấu
        food.StartCooking();

        // UI progress
        if (progressUI != null)
        {
            progressUI.SetFood(food);
        }

        cookingFoods.Add(food);
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

        food.StopCooking();

        food.transform.SetParent(null);

        Rigidbody rb = food.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        cookingFoods.Remove(food);

        return food.gameObject;
    }
}