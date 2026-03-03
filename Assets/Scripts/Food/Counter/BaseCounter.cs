using UnityEngine;

public class BaseCounter : MonoBehaviour
{
    [SerializeField] protected Transform foodPlacePoint;
    [SerializeField] protected Transform knifePlacePoint;

    protected GameObject currentObject;
    protected GameObject currentKnife;

    public virtual bool HasObject()
    {
        return currentObject != null;
    }

    public virtual void PlaceObject(GameObject obj)
    {
        // Nếu là Knife
        if (obj.GetComponent<Knife>() != null)
        {
            currentKnife = obj;

            obj.transform.SetParent(knifePlacePoint);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            return;
        }

        // Nếu là Food
        currentObject = obj;

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

    public virtual GameObject TakeObject()
    {
        GameObject obj = currentObject;
        currentObject = null;

        if (obj != null)
        {
            obj.transform.SetParent(null);
        }

        return obj;
    }

    public virtual void Interact(PlayerInteraction player)
    {
    }
}