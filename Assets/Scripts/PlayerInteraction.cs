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

        // Vẽ ray trong Scene để debug
        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red);

        if (Input.GetMouseButtonDown(0)) // Chuột trái
        {
            // Nếu đang cầm → thả
            if (heldObject != null)
            {
                DropObject();
                return;
            }

            // Nếu chưa cầm → thử nhặt
            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                // Quan trọng: dùng GetComponentInParent để bắt root object
                FoodItem food = hit.collider.GetComponentInParent<FoodItem>();

                if (food != null)
                {
                    PickUpObject(food.gameObject);
                }
                else
                {
                    Debug.Log("Ray hit but no FoodItem on this object: " + hit.collider.name);
                }
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