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
        return currentFood != null;
    }

    // =====================================================
    // PLACE OBJECT
    // =====================================================

    public virtual void PlaceObject(GameObject obj)
    {
        if (obj == null) return;

        // ===== KNIFE =====
        if (obj.TryGetComponent(out Knife knife))
        {
            if (currentKnife != null || knifePlacePoint == null) return;

            currentKnife = obj;
            PlaceAtPoint(obj, knifePlacePoint);
            return;
        }

        // ===== FOOD =====
        if (obj.TryGetComponent(out FoodItem food))
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
    // GET FOOD
    // =====================================================

    public GameObject GetFood()
    {
        return currentFood;
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

        Rigidbody rb = obj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // reset velocity trước
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // sau đó mới tắt physics
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        obj.transform.SetParent(point);

        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;

        Collider col = obj.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
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

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Collider col = obj.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }
    }

    public virtual void InteractAlternate(PlayerInteraction player)
    {
        // để trống
    }
}