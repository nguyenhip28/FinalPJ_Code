using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter
{
    [Header("Meat Slots")]
    [SerializeField] private List<Transform> meatPoints;

    [Header("UI")]
    [SerializeField] private CookingProgressUI progressUI;

    [Header("VFX")]
    [SerializeField] private ParticleSystem smokeEffect;
    [Header("Sound")]
    [SerializeField] private AudioSource sizzleSound;

    private List<FoodItem> cookingFoods = new List<FoodItem>();

    public override void Interact(PlayerInteraction player)
    {
        Debug.Log("Stove Interact Called");

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

        PlaceMeat(foodItem, freeSlot);

        player.ClearHeld();

        foodItem.StartCooking();

        if (progressUI != null)
        {
            progressUI.SetFood(foodItem);
        }

        cookingFoods.Add(foodItem);
        UpdateSmoke();
        UpdateSound();
    }

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

        Collider col = food.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        food.transform.SetParent(null);

        food.transform.localScale = Vector3.one;

        food.transform.position = slot.position;
        food.transform.rotation = slot.rotation;

        food.transform.SetParent(slot, true);
    }

    public GameObject TakeMeat(FoodItem food)
    {
        if (food == null)
            return null;

        food.StopCooking();

        food.transform.SetParent(null);

        food.transform.localScale = Vector3.one;

        Rigidbody rb = food.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Collider col = food.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        cookingFoods.Remove(food);
        UpdateSmoke();
        UpdateSound();

        return food.gameObject;
    }

    private void UpdateSmoke()
    {
        if (smokeEffect == null) return;

        if (cookingFoods.Count > 0)
        {
            if (!smokeEffect.isPlaying)
                smokeEffect.Play();
        }
        else
        {
            if (smokeEffect.isPlaying)
                smokeEffect.Stop();
        }
    }
    private void UpdateSound()
    {
        if (sizzleSound == null) return;

        if (cookingFoods.Count > 0)
        {
            if (!sizzleSound.isPlaying)
                sizzleSound.Play();
        }
        else
        {
            if (sizzleSound.isPlaying)
                sizzleSound.Stop();
        }
    }
}