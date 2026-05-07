using UnityEngine;

public class BaseCounter : MonoBehaviour, IInteractable
{
    [Header("Place Points")]
    [SerializeField] protected Transform foodPlacePoint;
    [SerializeField] protected Transform knifePlacePoint;

    protected GameObject currentFood;
    protected GameObject currentKnife;

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

    public virtual void PlaceObject(GameObject obj)
    {
        if (obj == null) return;

        if (obj.TryGetComponent(out Knife knife))
        {
            if (currentKnife != null || knifePlacePoint == null) return;

            currentKnife = obj;
            PlaceAtPoint(obj, knifePlacePoint);
            return;
        }

        if (obj.TryGetComponent(out FoodItem food))
        {
            if (currentFood != null || foodPlacePoint == null) return;

            currentFood = obj;
            PlaceAtPoint(obj, foodPlacePoint);
        }
    }

    public virtual GameObject TakeObject()
    {
        if (currentFood == null) return null;

        GameObject obj = currentFood;
        currentFood = null;

        DetachObject(obj);
        return obj;
    }

    public virtual GameObject TakeKnife()
    {
        if (currentKnife == null) return null;

        GameObject obj = currentKnife;
        currentKnife = null;

        DetachObject(obj);
        return obj;
    }

    public GameObject GetFood()
    {
        return currentFood;
    }

    public virtual void Interact(PlayerInteraction player)
    {
    }


    protected void PlaceAtPoint(GameObject obj, Transform point)
    {
        if (point == null) return;

        Rigidbody rb = obj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.isKinematic = true;
            rb.useGravity = false;
        }

        Vector3 originalScale = obj.transform.localScale;

        obj.transform.SetParent(point, true);

        obj.transform.position = point.position;
        obj.transform.rotation = point.rotation;

        obj.transform.localScale = originalScale;

        Collider col = obj.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
            col.isTrigger = false; 
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
    }
}