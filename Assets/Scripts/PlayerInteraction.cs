using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float interactDistance = 5f;
    public Camera playerCamera;
    public Transform holdPoint;

    private GameObject heldObject;

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red);

        if (Input.GetMouseButtonDown(0))
        {
            // =========================
            // 1️⃣ Nếu đang cầm đồ
            // =========================
            if (heldObject != null)
            {
                if (Physics.Raycast(ray, out hit, interactDistance))
                {
                    // 🔹 Kiểm tra Counter (bàn)
                    KitchenCounter counter = hit.collider.GetComponentInParent<KitchenCounter>();

                    if (counter != null && !counter.HasFood())
                    {
                        counter.PlaceFood(heldObject);
                        heldObject = null;
                        return;
                    }
                }

                // Nếu không trúng bàn → thả xuống đất
                DropObject();
                return;
            }

            // =========================
            // 2️⃣ Nếu chưa cầm đồ
            // =========================
            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                // 🔹 Thử nhặt Food
                FoodItem food = hit.collider.GetComponentInParent<FoodItem>();

                if (food != null)
                {
                    PickUpObject(food.gameObject);
                    return;
                }

                // 🔹 Thử lấy đồ từ Counter
                KitchenCounter counter = hit.collider.GetComponentInParent<KitchenCounter>();

                if (counter != null && counter.HasFood())
                {
                    GameObject foodFromCounter = counter.TakeFood();
                    PickUpObject(foodFromCounter);
                    return;
                }

                Debug.Log("Ray hit: " + hit.collider.name);
            }
        }
    }

    void PickUpObject(GameObject obj)
    {
        heldObject = obj;

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        heldObject.transform.SetParent(holdPoint);
        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;

        Debug.Log("Picked up: " + obj.name);
    }

    void DropObject()
    {
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();

        heldObject.transform.SetParent(null);

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        Debug.Log("Dropped: " + heldObject.name);

        heldObject = null;
    }
}