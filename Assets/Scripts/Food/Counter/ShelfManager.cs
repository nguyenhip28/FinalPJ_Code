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
                // lưu item vào slot
                storedItems[i] = item;

                // đặt vị trí
                item.transform.position = points[i].position;

                // quay mặt logo ra ngoài (offset -90 như bạn test)
                item.transform.rotation =
                    Quaternion.LookRotation(points[i].forward) *
                    Quaternion.Euler(0, -90, 0);

                // gắn vào point
                item.transform.SetParent(points[i]);

                // fix physics
                Rigidbody rb = item.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }

                // 👉 gắn thông tin slot vào item
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