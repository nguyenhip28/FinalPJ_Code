using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter
{
    [Header("Meat Slots")]
    [SerializeField] private List<Transform> meatPoints;

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

        PlaceMeat(food, freeSlot);

        player.ClearHeld();

        food.StartCooking();

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
                return slot;
        }

        return null;
    }

    // =====================================================
    // PLACE MEAT (ANTI TELEPORT + ANTI SCALE BUG)
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

        // CẮT KHỎI PARENT CŨ
        food.transform.SetParent(null);

        // Reset scale
        food.transform.localScale = Vector3.one;

        // Đặt world position
        food.transform.position = slot.position;
        food.transform.rotation = slot.rotation;

        // Parent lại
        food.transform.SetParent(slot, true);
    }

    // =====================================================
    // OPTIONAL: TAKE MEAT BACK
    // =====================================================

    public GameObject TakeMeat(FoodItem food)
    {
        food.transform.SetParent(null);

        Rigidbody rb = food.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;   // bật lại
            rb.useGravity = true;
        }

        return food.gameObject;
    }
}