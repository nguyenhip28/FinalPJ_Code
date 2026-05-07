using UnityEngine;

public class ShelfManager : MonoBehaviour
{
    [Header("Shelf Points")]
    public Transform[] points;

    private GameObject[] storedItems;

    void Start()
    {
        storedItems = new GameObject[points.Length];
    }

    public bool TryStore(GameObject item)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (storedItems[i] == null)
            {
                storedItems[i] = item;

                Rigidbody rb = item.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }

                item.transform.SetParent(points[i], false);
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.Euler(0, -90, 0);

                ShelfItem shelfItem = item.GetComponent<ShelfItem>();

                if (shelfItem == null)
                {
                    shelfItem = item.AddComponent<ShelfItem>();
                }

                shelfItem.shelf = this;
                shelfItem.index = i;

                return true;
            }
        }

        Debug.Log("Shelf Full!");
        return false;
    }

    public void ClearSlot(int index)
    {
        if (index >= 0 && index < storedItems.Length)
        {
            storedItems[index] = null;
        }
    }
}