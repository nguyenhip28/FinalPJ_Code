using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float interactDistance = 3f;
    public Camera playerCamera;
    public Transform holdPoint;

    [Header("Knife Visual (FPS)")]
    public GameObject knifeVisual;

    private BaseCounter selectedCounter;

    private GameObject heldObject;
    private Knife heldKnife;

    void Start()
    {
        if (knifeVisual != null)
            knifeVisual.SetActive(false);
    }

    void Update()
    {
        HandleRaycast();
        HandlePrimaryAction();

        // ===== PHÍM E INTERACT =====
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (selectedCounter != null)
                selectedCounter.Interact(this);
        }
    }

    // =====================================================
    // RAYCAST
    // =====================================================
    void HandleRaycast()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            selectedCounter = hit.collider.GetComponentInParent<BaseCounter>();
        }
        else
        {
            selectedCounter = null;
        }

        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red);
    }

    // =====================================================
    // LEFT CLICK
    // =====================================================
    void HandlePrimaryAction()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        // ===== INTERACT WITH COUNTER =====
        if (selectedCounter != null)
        {
            if (!IsHoldingAnything())
            {
                if (selectedCounter.HasObject())
                {
                    PickUp(selectedCounter.TakeObject());
                }
            }
            else
            {
                if (!selectedCounter.HasObject())
                {
                    selectedCounter.PlaceObject(GetHeldGameObject());
                    ClearHeld();
                }
            }

            return;
        }

        // ===== PICK DIRECT OBJECT =====
        if (!IsHoldingAnything())
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                if (hit.collider.CompareTag("Food") ||
                    hit.collider.GetComponent<Knife>() != null)
                {
                    PickUp(hit.collider.gameObject);
                }
            }
        }
        else
        {
            DropToGround();
        }
    }

    // =====================================================
    // PICK UP
    // =====================================================
    void PickUp(GameObject obj)
    {
        if (obj == null) return;

        Knife knife = obj.GetComponent<Knife>();

        // ===== PICK KNIFE =====
        if (knife != null)
        {
            heldKnife = knife;

            obj.SetActive(false);

            if (knifeVisual != null)
                knifeVisual.SetActive(true);

            return;
        }

        // ===== PICK FOOD =====
        heldObject = obj;

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        obj.transform.SetParent(holdPoint);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
    }

    // =====================================================
    // DROP
    // =====================================================
    void DropToGround()
    {
        // DROP KNIFE
        if (heldKnife != null)
        {
            heldKnife.gameObject.SetActive(true);
            heldKnife = null;

            if (knifeVisual != null)
                knifeVisual.SetActive(false);

            return;
        }

        // DROP FOOD
        if (heldObject != null)
        {
            heldObject.transform.SetParent(null);

            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }

            heldObject = null;
        }
    }

    // =====================================================
    // API
    // =====================================================

    public GameObject GetHeldObject()
    {
        return heldObject;
    }

    public GameObject GetHeldGameObject()
    {
        if (heldObject != null) return heldObject;
        if (heldKnife != null) return heldKnife.gameObject;
        return null;
    }

    public bool IsHoldingObject()
    {
        return heldObject != null;
    }

    public bool HasKnife()
    {
        return heldKnife != null;
    }

    public bool IsHoldingAnything()
    {
        return heldObject != null || heldKnife != null;
    }

    public void ClearHeld()
    {
        if (heldKnife != null)
        {
            heldKnife.gameObject.SetActive(true);
            heldKnife = null;

            if (knifeVisual != null)
                knifeVisual.SetActive(false);
        }

        heldObject = null;
    }
}