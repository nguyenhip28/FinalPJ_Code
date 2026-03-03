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
            if (currentKnife != null) return;

            currentKnife = obj;
            PlaceAtPoint(obj, knifePlacePoint);
            return;
        }

        // ===== FOOD =====
        FoodItem food = obj.GetComponent<FoodItem>();
        if (food != null)
        {
            if (currentFood != null) return;

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
    // INTERACT (Override ở class con)
    // =====================================================

    public virtual void Interact(PlayerInteraction player)
    {
        // BaseCounter mặc định không làm gì
    }

    // =====================================================
    // HELPER FUNCTIONS
    // =====================================================

    protected void PlaceAtPoint(GameObject obj, Transform point)
    {
        obj.transform.SetParent(point);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
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