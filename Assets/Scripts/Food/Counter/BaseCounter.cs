using UnityEngine;

public class BaseCounter : MonoBehaviour
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

        // ===== PLACE KNIFE =====
        Knife knife = obj.GetComponent<Knife>();
        if (knife != null)
        {
            if (currentKnife != null) return; // Không cho đặt đè

            currentKnife = obj;

            obj.transform.SetParent(knifePlacePoint);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;

            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            return;
        }

        // ===== PLACE FOOD =====
        FoodItem food = obj.GetComponent<FoodItem>();
        if (food != null)
        {
            if (currentFood != null) return; // Không cho đặt đè

            currentFood = obj;

            obj.transform.SetParent(foodPlacePoint);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;

            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }
    }

    // =====================================================
    // TAKE FOOD
    // =====================================================

    public virtual GameObject TakeObject()
    {
        GameObject obj = currentFood;
        currentFood = null;

        if (obj != null)
        {
            obj.transform.SetParent(null);

            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
        }

        return obj;
    }

    // =====================================================
    // TAKE KNIFE
    // =====================================================

    public virtual GameObject TakeKnife()
    {
        GameObject obj = currentKnife;
        currentKnife = null;

        if (obj != null)
        {
            obj.transform.SetParent(null);

            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
        }

        return obj;
    }

    // =====================================================
    // INTERACT (Override ở class con)
    // =====================================================

    public virtual void Interact(PlayerInteraction player)
    {
    }
}