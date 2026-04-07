using UnityEngine;
using TMPro;

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

    [Header("UI")]
    public TextMeshProUGUI hintText;

    private Transform currentHoldPoint;
    private float holdDistance = 1.5f;
    public LayerMask computerLayer;

    public LayerMask shelfLayer;

    [Header("Highlight")]
    public LayerMask highlightLayer;

    private Outline currentOutline;


    void Start()
    {
        if (knifeVisual != null)
            knifeVisual.SetActive(false);
    }

    void Update()
    {
        HandleHighlight();
        HandleRaycast();

        bool showHint = false;

        // Hiện hint khi cầm meat và nhìn vào stove
        if (selectedCounter is StoveCounter && heldObject != null)
        {
            FoodItem food = heldObject.GetComponent<FoodItem>();

            if (food != null && food.foodType == FoodType.Meat)
            {
                showHint = true;
            }
        }

        if (hintText != null)
        {
            hintText.gameObject.SetActive(showHint);

            if (showHint)
                hintText.text = "[E] Cook Meat";
        }

        HandlePrimaryAction();

        if (Input.GetKeyDown(KeyCode.E))
        {
            // ===== COMPUTER INTERACTION (ƯU TIÊN CAO NHẤT) =====
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            RaycastHit hit;

            // 👇 CHỈ BẮN VÀO LAYER COMPUTER
            if (Physics.Raycast(ray, out hit, interactDistance, computerLayer))
            {
                ComputerInteractionAdvanced computer = hit.collider.GetComponentInParent<ComputerInteractionAdvanced>();

                if (computer != null)
                {
                    computer.TryInteract();
                    return;
                }
            }

            // ===== SHELF STORAGE =====
            if (heldObject != null)
            {
                if (Physics.Raycast(ray, out hit, interactDistance, shelfLayer))
                {
                    ShelfManager shelf = hit.collider.GetComponentInParent<ShelfManager>();

                    if (shelf != null)
                    {
                        GameObject obj = heldObject;

                        bool stored = shelf.TryStore(obj);

                        if (stored)
                        {
                            // ✅ KHÔNG detach nữa (ShelfManager đã parent rồi)

                            // ✅ reset rigidbody đúng cách
                            Rigidbody rb = obj.GetComponent<Rigidbody>();
                            if (rb != null)
                            {
                                rb.isKinematic = true;
                                rb.useGravity = false;
                                rb.linearVelocity = Vector3.zero;         // FIX
                                rb.angularVelocity = Vector3.zero;
                            }

                            // ✅ clear state player
                            heldObject = null;
                            currentHoldPoint = null;
                        }

                        return; // ❗ chặn toàn bộ logic dưới
                    }
                }
            }

            // ===== CẦM BOX + NHÌN TRAY =====
            if (heldObject != null && heldObject.GetComponent<FoodBox>() != null)
            {
                if (selectedCounter is TrayCounter tray)
                {
                    FoodBox box = heldObject.GetComponent<FoodBox>();

                    if (box.HasFood() && !tray.IsFull())
                    {
                        tray.AddFood(box.foodPrefab);
                        box.UseOne();
                    }

                    return;
                }
            }

            // ===== CHẶN E VỚI FOODBOX =====
            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                if (hit.collider.GetComponent<FoodBox>() != null)
                {
                    return;
                }
            }

            // ===== LOGIC CŨ =====
            if (selectedCounter != null)
            {
                selectedCounter.Interact(this);
            }
        }
    }
    void LateUpdate()
    {
        if (heldObject != null && currentHoldPoint != null)
        {
            heldObject.transform.position = currentHoldPoint.position;
            heldObject.transform.rotation = currentHoldPoint.rotation;
        }
    }

    void HandleHighlight()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, highlightLayer))
        {
            Outline outline = hit.collider.GetComponentInParent<Outline>();

            if (outline != currentOutline)
            {
                ClearHighlight();

                if (outline != null)
                {
                    outline.enabled = true;   // 👈 bật
                    currentOutline = outline;
                }
            }
        }
        else
        {
            ClearHighlight();
        }
    }

    void ClearHighlight()
    {
        if (currentOutline != null)
        {
            currentOutline.enabled = false;  // 👈 tắt
            currentOutline = null;
        }
    }

    // =====================================================
    // RAYCAST
    // =====================================================

    void HandleRaycast()
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

    // =====================================================
    // LEFT CLICK
    // =====================================================

    void HandlePrimaryAction()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // ===== BLOCK COOK =====
        if (selectedCounter is StoveCounter && heldObject != null)
        {
            FoodItem food = heldObject.GetComponent<FoodItem>();

            if (food != null && food.foodType == FoodType.Meat)
            {
                if (hintText != null)
                {
                    hintText.gameObject.SetActive(true);
                    hintText.text = "Press E to cook meat";
                }
                return;
            }
        }

        // ===== CLICK OBJECT (BOX / FOOD / KNIFE) =====

        // 👉 TRƯỜNG HỢP ĐANG CẦM → CLICK LÀ DROP LUÔN
        if (heldObject != null || heldKnife != null)
        {
            DropToGround();
            return;
        }

        // 👉 KHÔNG CẦM GÌ → MỚI ĐI PICK
        int pickLayer = LayerMask.GetMask("HoldItem");

        if (Physics.Raycast(ray, out hit, interactDistance, pickLayer))
        {
            GameObject target = hit.collider.gameObject;

            // 👉 Ưu tiên lấy FoodBox parent
            FoodBox box = hit.collider.GetComponentInParent<FoodBox>();
            if (box != null)
            {
                target = box.gameObject;
            }

            // 👉 CHECK ĐÚNG LOẠI OBJECT
            if (target.CompareTag("Food") ||
                target.CompareTag("Pickup") ||
                target.GetComponent<Knife>() != null ||
                box != null)
            {
                PickUp(target);
                return;
            }
        }

        // ===== INTERACT COUNTER =====
        if (selectedCounter != null)
        {
            if (heldObject == null)
            {
                if (selectedCounter.HasFood())
                {
                    // 👉 Ưu tiên lấy đúng object mình click
                    if (Physics.Raycast(ray, out hit, interactDistance))
                    {
                        FoodItem food = hit.collider.GetComponentInParent<FoodItem>();

                        if (food != null && selectedCounter is TrayCounter tray)
                        {
                            GameObject picked = tray.TakeSpecific(food.gameObject);

                            if (picked != null)
                            {
                                PickUp(picked);
                                return;
                            }
                        }
                    }

                    // 👉 fallback an toàn (KHÔNG BAO GIỜ CRASH)
                    GameObject fallback = selectedCounter.TakeObject();

                    if (fallback != null)
                    {
                        PickUp(fallback);
                    }
                    else
                    {
                        Debug.LogWarning("TakeObject trả về NULL");
                    }
                }
            }
            else
            {
                FoodItem food = heldObject.GetComponent<FoodItem>();

                if (food != null && !selectedCounter.HasFood())
                {
                    GameObject obj = heldObject;

                    selectedCounter.PlaceObject(obj);

                    heldObject = null;
                }
            }
        }
    }

    // =====================================================
    // PICK UP
    // =====================================================

    public Transform boxPoint; // kéo trong Inspector

    public void PickUp(GameObject obj)
    {
        // ===== SAFETY CHECK =====
        if (obj == null)
        {
            Debug.LogError("PickUp nhận obj NULL");
            return;
        }

        // ===== CLEAR SHELF (nếu có) =====
        ShelfItem shelfItem = obj.GetComponent<ShelfItem>();

        if (shelfItem != null)
        {
            if (shelfItem.shelf != null)
            {
                shelfItem.shelf.ClearSlot(shelfItem.index);
            }

            // delay destroy để tránh lỗi frame
            Destroy(shelfItem, 0.01f);
        }

        // ===== PICK KNIFE =====
        Knife knife = obj.GetComponent<Knife>();

        if (knife != null)
        {
            heldKnife = knife;

            obj.SetActive(false);

            if (knifeVisual != null)
                knifeVisual.SetActive(true);

            return;
        }

        // ===== PICK OBJECT =====
        heldObject = obj;

        // ===== RESET PHYSICS =====
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // ===== CHỌN HOLD POINT =====
        Transform targetPoint = holdPoint;

        if (obj.GetComponent<FoodBox>() != null && boxPoint != null)
        {
            targetPoint = boxPoint;
        }

        if (targetPoint == null)
        {
            Debug.LogError("HoldPoint NULL");
            return;
        }

        currentHoldPoint = targetPoint;

        // ===== DETACH TRƯỚC KHI GẮN =====
        obj.transform.SetParent(null, false);

        // ===== GẮN VÀO TAY =====
        obj.transform.SetParent(currentHoldPoint, false);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        obj.transform.localScale = Vector3.one;
    }
    // =====================================================
    // DROP
    // =====================================================

    void DropToGround()
    {
        if (heldKnife != null)
        {
            heldKnife.gameObject.SetActive(true);
            heldKnife = null;

            if (knifeVisual != null)
                knifeVisual.SetActive(false);

            return;
        }

        if (heldObject != null)
        {
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.linearVelocity = Vector3.zero;
            }

            // ✅ FIX: bỏ parent
            heldObject.transform.SetParent(null);

            heldObject = null;
            currentHoldPoint = null;
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