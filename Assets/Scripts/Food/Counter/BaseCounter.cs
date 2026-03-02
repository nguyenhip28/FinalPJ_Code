using UnityEngine;

public class BaseCounter : MonoBehaviour
{
    [SerializeField] protected Transform placePoint;

    protected GameObject currentObject;

    public virtual bool HasObject()
    {
        return currentObject != null;
    }

    public virtual void PlaceObject(GameObject obj)
    {
        currentObject = obj;

        obj.transform.SetParent(placePoint);
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
}