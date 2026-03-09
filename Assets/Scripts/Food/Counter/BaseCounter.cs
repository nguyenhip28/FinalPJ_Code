using UnityEngine;

public class BaseCounter : MonoBehaviour, IInteractable
{
    [Header("Place Points")]
    [SerializeField] protected Transform foodPlacePoint;
    [SerializeField] protected Transform knifePlacePoint;

    protected GameObject currentFood;
    protected GameObject currentKnife;

    // =====================================================
    // CHECK STATE
    // =====================================================

    public virtual bool HasFood()
    {
        return currentFood != null;
    }

    public virtual bool HasKnife()
    {
        return currentKnife != null;
    }

    public virtual bool HasObject()
    {
        return currentFood != null || currentKnife != null;
    }

    // =====================================================
    // PLACE OBJECT
    // =====================================================

    public virtual void PlaceObject(GameObject obj)
    {
        if (obj == null) return;

        // ===== KNIFE =====
        Knife knife = obj.GetComponent<Knife>();
        if (knife != null)
        {
            if (currentKnife != null || knifePlacePoint == null) return;

            currentKnife = obj;
            PlaceAtPoint(obj, knifePlacePoint);
            return;
        }

        // ===== FOOD =====
        FoodItem food = obj.GetComponent<FoodItem>();
        if (food != null)
        {
            if (currentFood != null || foodPlacePoint == null) return;

            currentFood = obj;
            PlaceAtPoint(obj, foodPlacePoint);
        }
    }

    // =====================================================
    // TAKE FOOD
    // =====================================================

    public virtual GameObject TakeObject()
    {
        if (currentFood == null) return null;

        GameObject obj = currentFood;
        currentFood = null;

        DetachObject(obj);
        return obj;
    }

    // =====================================================
    // TAKE KNIFE
    // =====================================================

    public virtual GameObject TakeKnife()
    {
        if (currentKnife == null) return null;

        GameObject obj = currentKnife;
        currentKnife = null;

        DetachObject(obj);
        return obj;
    }

    // =====================================================
    // INTERACT
    // =====================================================

    public virtual void Interact(PlayerInteraction player)
    {
        // Override ở class con
    }

    // =====================================================
    // HELPER FUNCTIONS
    // =====================================================

    protected void PlaceAtPoint(GameObject obj, Transform point)
    {
        if (point == null) return;

        obj.transform.SetParent(point, false);

        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    protected void DetachObject(GameObject obj)
    {
        obj.transform.SetParent(null);

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}