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

    // =====================================================
    // STORE ITEM
    // =====================================================
    public bool TryStore(GameObject item)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (storedItems[i] == null)
            {
                storedItems[i] = item;

                // 👉 Physics trước
                Rigidbody rb = item.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;          // FIX
                    rb.angularVelocity = Vector3.zero;
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }

                // 👉 Parent đúng cách
                item.transform.SetParent(points[i], false);
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.Euler(0, -90, 0);

                // ✅ GẮN ShelfItem ĐÚNG CHỖ
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


    // =====================================================
    // CLEAR SLOT (khi lấy item)
    // =====================================================
    public void ClearSlot(int index)
    {
        if (index >= 0 && index < storedItems.Length)
        {
            storedItems[index] = null;
        }
    }
}