using SojaExiles;
using TMPro;
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

    [Header("UI")]
    public TextMeshProUGUI hintText;

    private Transform currentHoldPoint;
    private float holdDistance = 1.5f;
    public LayerMask computerLayer;

    public LayerMask shelfLayer;

    [Header("Highlight")]
    public LayerMask highlightLayer;
    private Outline currentOutline;

    [Header("UI Layer")]
    public LayerMask uiInteractLayer;

    public LayerMask doorLayer;

    void Start()
    {
        if (knifeVisual != null)
            knifeVisual.SetActive(false);

        if (hintText != null)
            hintText.gameObject.SetActive(false); // 👈 thêm dòng này
    }

    void Update()
    {
        HandleHighlight();
        HandleRaycast();

        Ray rayUI = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hitUI;

        bool showHint = false;
        string hinte = "";

        // ===== CHECK COMPUTER MODE =====
        ComputerInteractionAdvanced computerMain = FindObjectOfType<ComputerInteractionAdvanced>();

        if (computerMain != null && computerMain.isUsingComputer)
        {
            if (hintText != null)
                hintText.gameObject.SetActive(false);

            // ❗ KHÔNG return nếu bên dưới còn logic quan trọng
        }
        else
        {
            // ===== ƯU TIÊN: COOK =====
            if (selectedCounter is StoveCounter && heldObject != null)
            {
                FoodItem food = heldObject.GetComponent<FoodItem>();

                if (food != null && food.foodType == FoodType.Meat)
                {
                    showHint = true;
                    hinte = "[E] Cook Meat";
                }
            }

            // ===== DEFAULT INTERACTION =====
            else if (Physics.Raycast(rayUI, out hitUI, interactDistance, uiInteractLayer))
            {
                BaseCounter counter = hitUI.collider.GetComponentInParent<BaseCounter>();
                FoodItem food = hitUI.collider.GetComponentInParent<FoodItem>();
                ComputerInteractionAdvanced computerHit = hitUI.collider.GetComponentInParent<ComputerInteractionAdvanced>();

                if (counter != null || food != null || computerHit != null)
                {
                    showHint = true;

                    if (heldObject == null)
                    {
                        hinte = "[E] Interact\n[Click] Pick up";
                    }
                    else
                    {
                        hinte = "[E] Interact\n[Click] Drop";
                    }
                }
            }

            // ===== HIỂN THỊ =====
            if (hintText != null)
            {
                hintText.gameObject.SetActive(showHint);
                hintText.text = hinte;
            }
        }

        HandlePrimaryAction();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (hintText != null)
            {
                hintText.gameObject.SetActive(false);
            }

            // ===== DOOR INTERACTION =====
            Ray rayDoor = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            RaycastHit hitDoor;

            if (Physics.Raycast(rayDoor, out hitDoor, interactDistance))
            {
                // 👉 Nếu nhìn vào bảng OPEN
                if (hitDoor.collider.CompareTag("DoorSwitch"))
                {
                    DoorSwitch switcher = hitDoor.collider.GetComponent<DoorSwitch>();

                    if (switcher != null)
                    {
                        switcher.ToggleBothDoors();
                        return;
                    }
                }

                // 👉 Nếu nhìn trực tiếp vào cửa
                opencloseDoor door = hitDoor.collider.GetComponentInParent<opencloseDoor>();

                if (door != null)
                {
                    door.ToggleDoor();
                    return;
                }
            }

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
                            Rigidbody rb = obj.GetComponent<Rigidbody>();
                            if (rb != null)
                            {
                                rb.isKinematic = true;
                                rb.useGravity = false;
                                rb.linearVelocity = Vector3.zero;
                                rb.angularVelocity = Vector3.zero;
                            }

                            // ✅ reset layer
                            if (obj.GetComponent<FoodItem>() != null)
                            {
                                obj.layer = LayerMask.NameToLayer("Food");
                            }
                            else
                            {
                                obj.layer = LayerMask.NameToLayer("HoldItem");
                            }

                            // ✅ collider
                            Collider col = obj.GetComponent<Collider>();
                            if (col != null)
                            {
                                col.enabled = true;
                                col.isTrigger = false;
                            }

                            // (optional nhưng an toàn)
                            obj.transform.SetParent(null);

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

        int interactLayer = LayerMask.GetMask("Counter", "HoldItem", "Food");

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
        int pickLayer = LayerMask.GetMask("HoldItem", "Food");

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
            // ❗ Nếu đang nhìn tray thì KHÔNG pick trực tiếp
            if (selectedCounter is TrayCounter)
            {
                // bỏ qua → xử lý phía dưới
            }
            else
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
        }

        // ===== INTERACT COUNTER =====

        if (selectedCounter != null)
        {
            // ===== KHÔNG CẦM → LẤY =====
            if (heldObject == null)
            {
                if (selectedCounter.HasFood())
                {
                    RaycastHit[] hits = Physics.RaycastAll(ray, interactDistance);

                    FoodItem targetFood = null;
                    float closest = float.MaxValue;

                    foreach (var h in hits)
                    {
                        FoodItem f = h.collider.GetComponentInParent<FoodItem>();

                        if (f != null)
                        {
                            // 👉 luôn lấy ROOT object của food
                            GameObject rootFood = f.gameObject;

                            float dist = Vector3.Distance(ray.origin, h.point);

                            if (dist < closest)
                            {
                                closest = dist;
                                targetFood = f;
                            }
                        }
                    }

                    // ✅ Tray → lấy đúng cái crosshair nhìn vào
                    if (selectedCounter is TrayCounter tray)
                    {
                        if (tray.HasFood())
                        {
                            RaycastHit[] hites = Physics.RaycastAll(ray, interactDistance);

                            FoodItem closestFood = null;
                            float minDist = float.MaxValue;

                            foreach (var h in hites)
                            {
                                FoodItem f = h.collider.GetComponentInParent<FoodItem>();
                                if (f != null && h.distance < minDist)
                                {
                                    minDist = h.distance;
                                    closestFood = f;
                                }
                            }

                            if (closestFood != null)
                            {
                                GameObject picked = tray.TakeSpecific(closestFood.gameObject);
                                if (picked != null)
                                {
                                    PickUp(picked);
                                    return;
                                }
                            }
                        }
                        return;
                    }

                    // fallback nếu KHÔNG phải tray
                    if (!(selectedCounter is TrayCounter))
                    {
                        GameObject fallback = selectedCounter.TakeObject();

                        if (fallback != null)
                        {
                            PickUp(fallback);
                        }
                    }
                }
            }
            // ===== ĐANG CẦM → ĐẶT =====
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
        // ✅ Khi cầm → đổi sang layer "Ignore Raycast" để không bị raycast hit
        heldObject = obj;
        obj.layer = LayerMask.NameToLayer("Ignore Raycast");

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
            GameObject obj = heldObject;

            Rigidbody rb = obj.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // ✅ QUAN TRỌNG: bỏ parent
            obj.transform.SetParent(null);

            // ✅ RESET LAYER (để raycast lại được)
            FoodItem food = obj.GetComponent<FoodItem>();

            if (food != null)
            {
                obj.layer = LayerMask.NameToLayer("Food");
            }
            else
            {
                obj.layer = LayerMask.NameToLayer("HoldItem");
            }

            // ✅ đảm bảo collider bật
            Collider col = obj.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = true;
                col.isTrigger = false;
            }

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