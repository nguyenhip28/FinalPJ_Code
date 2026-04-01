using UnityEngine;

public class PlayerInteractionController
{
    private PlayerInteractionModel model;
    private PlayerInteractionView view;

    private Camera playerCamera;
    private float interactDistance;

    private Transform holdPoint;
    private Transform boxPoint;

    private BaseCounter selectedCounter;

    public PlayerInteractionController(
        PlayerInteractionModel model,
        PlayerInteractionView view,
        Camera cam,
        float distance,
        Transform holdPoint,
        Transform boxPoint
    )
    {
        this.model = model;
        this.view = view;
        this.playerCamera = cam;
        this.interactDistance = distance;
        this.holdPoint = holdPoint;
        this.boxPoint = boxPoint;
    }

    // ========================= RAYCAST =========================
    public void HandleRaycast()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        int interactLayer = LayerMask.GetMask("Counter", "HoldItem");

        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            selectedCounter = hit.collider.GetComponentInParent<BaseCounter>();
        }
        else
        {
            selectedCounter = null;
        }

        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red);
    }

    // ========================= UPDATE =========================
    public void Update()
    {
        HandleRaycast();
        HandleHint();
        HandleInput();
    }

    void HandleHint()
    {
        bool showHint = false;

        if (selectedCounter is StoveCounter && model.heldObject != null)
        {
            FoodItem food = model.heldObject.GetComponent<FoodItem>();

            if (food != null && food.foodType == FoodType.Meat)
            {
                showHint = true;
            }
        }

        if (showHint)
            view.ShowHint("[E] Cook Meat");
        else
            view.HideHint();
    }

    void HandleInput()
    {
        HandlePrimaryAction();

        if (Input.GetKeyDown(KeyCode.E))
        {
            HandleInteractKey();
        }
    }

    // ========================= E KEY =========================
    void HandleInteractKey()
    {
        if (model.heldObject != null && model.heldObject.GetComponent<FoodBox>() != null)
        {
            if (selectedCounter is TrayCounter tray)
            {
                FoodBox box = model.heldObject.GetComponent<FoodBox>();

                if (box.HasFood() && !tray.IsFull())
                {
                    tray.AddFood(box.foodPrefab);
                    box.UseOne();
                }

                return;
            }
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            if (hit.collider.GetComponent<FoodBox>() != null)
            {
                return;
            }
        }

        if (selectedCounter != null)
        {
            selectedCounter.Interact(null); // có thể refactor sau
        }
    }

    // ========================= LEFT CLICK =========================
    void HandlePrimaryAction()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (selectedCounter is StoveCounter && model.heldObject != null)
        {
            FoodItem food = model.heldObject.GetComponent<FoodItem>();

            if (food != null && food.foodType == FoodType.Meat)
            {
                view.ShowHint("Press E to cook meat");
                return;
            }
        }

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            GameObject target = hit.collider.gameObject;

            FoodBox box = hit.collider.GetComponentInParent<FoodBox>();
            if (box != null)
                target = box.gameObject;

            if (!model.IsHoldingAnything())
            {
                if (target.CompareTag("Food") ||
                    target.CompareTag("Pickup") ||
                    target.GetComponent<Knife>() != null ||
                    box != null)
                {
                    PickUp(target);
                    return;
                }
            }
            else
            {
                Drop();
                return;
            }
        }

        if (selectedCounter != null)
        {
            if (!model.IsHoldingObject())
            {
                if (selectedCounter.HasFood())
                {
                    PickUp(selectedCounter.TakeObject());
                }
            }
            else
            {
                FoodItem food = model.heldObject.GetComponent<FoodItem>();

                if (food != null && !selectedCounter.HasFood())
                {
                    GameObject obj = model.heldObject;

                    selectedCounter.PlaceObject(obj);
                    model.heldObject = null;
                }
            }
        }
    }

    // ========================= PICK =========================
    void PickUp(GameObject obj)
    {
        if (obj == null) return;

        Knife knife = obj.GetComponent<Knife>();

        if (knife != null)
        {
            model.heldKnife = knife;
            obj.SetActive(false);
            view.ShowKnife(true);
            return;
        }

        model.heldObject = obj;

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Transform targetPoint = holdPoint;

        if (obj.GetComponent<FoodBox>() != null && boxPoint != null)
        {
            targetPoint = boxPoint;
        }

        model.currentHoldPoint = targetPoint;

        obj.transform.SetParent(targetPoint);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
    }

    // ========================= DROP =========================
    void Drop()
    {
        if (model.heldKnife != null)
        {
            model.heldKnife.gameObject.SetActive(true);
            model.heldKnife = null;
            view.ShowKnife(false);
            return;
        }

        if (model.heldObject != null)
        {
            Rigidbody rb = model.heldObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.linearVelocity = Vector3.zero;
            }

            model.heldObject.transform.SetParent(null);

            model.heldObject = null;
            model.currentHoldPoint = null;
        }
    }

    // ========================= LATE UPDATE =========================
    public void LateUpdate()
    {
        if (model.heldObject != null && model.currentHoldPoint != null)
        {
            model.heldObject.transform.position = model.currentHoldPoint.position;
            model.heldObject.transform.rotation = model.currentHoldPoint.rotation;
        }
    }
}