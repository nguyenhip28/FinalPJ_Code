using UnityEngine;
using System.Collections.Generic;

public class TrayCounter : BaseCounter
{
    [Header("Food Slots")]
    public Transform[] foodPoints; // 6 slot

    private List<GameObject> currentFoods = new List<GameObject>();

    // =====================================================
    // CHECK
    // =====================================================

    public bool IsFull()
    {
        return currentFoods.Count >= foodPoints.Length;
    }

    public override bool HasFood()
    {
        return currentFoods.Count > 0;
    }

    // =====================================================
    // PLACE OBJECT (QUAN TRỌNG NHẤT)
    // =====================================================

    public override void PlaceObject(GameObject obj)
    {
        if (obj == null) return;

        // chỉ nhận FoodItem
        if (!obj.TryGetComponent(out FoodItem food)) return;

        // full rồi thì không cho đặt
        if (IsFull()) return;

        Transform point = foodPoints[currentFoods.Count];

        currentFoods.Add(obj);

        // dùng base để set position + tắt physics
        PlaceAtPoint(obj, point);
    }

    // =====================================================
    // ADD FOOD (dùng cho FoodBox -> E)
    // =====================================================

    public void AddFood(GameObject foodPrefab)
    {
        if (IsFull()) return;

        Transform point = foodPoints[currentFoods.Count];

        GameObject food = Instantiate(foodPrefab);

        currentFoods.Add(food);

        PlaceAtPoint(food, point);
    }

    // =====================================================
    // TAKE FOOD (lấy ra 1 cái - LIFO)
    // =====================================================

    public GameObject TakeObjectByPlayer(Transform player)
    {
        if (currentFoods.Count == 0) return null;

        GameObject closest = null;
        float minDist = float.MaxValue;

        foreach (var food in currentFoods)
        {
            float dist = Vector3.Distance(player.position, food.transform.position);

            if (dist < minDist)
            {
                minDist = dist;
                closest = food;
            }
        }

        if (closest != null)
        {
            currentFoods.Remove(closest);
            DetachObject(closest);
        }

        return closest;
    }

    // =====================================================
    // INTERACT (optional debug)
    // =====================================================

    public override void Interact(PlayerInteraction player)
    {
        Debug.Log("TrayCounter Interact");
    }
    public GameObject TakeSpecific(GameObject target)
    {
        if (target == null) return null;

        if (currentFoods.Contains(target))
        {
            currentFoods.Remove(target);
            DetachObject(target);
            return target;
        }

        return null;
    }
    public override GameObject TakeObject()
    {
        if (currentFoods.Count == 0) return null;

        // lấy item cuối (LIFO)
        GameObject obj = currentFoods[currentFoods.Count - 1];

        currentFoods.RemoveAt(currentFoods.Count - 1);

        DetachObject(obj);

        return obj;
    }
}