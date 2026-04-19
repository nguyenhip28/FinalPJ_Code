using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class NPCMovement : MonoBehaviour
{
    public enum NPCState
    {
        WalkingPath,
        GoingToQueue,
        WaitingInQueue,
        Ordering,
        LeavingShop
    }

    private NPCState currentState = NPCState.WalkingPath;

    private NPCQueueManager queueManager;
    private Transform queueTarget;
    private Transform orderPoint;
    private Transform exitPoint;

    public WaypointManager path;

    [Header("Order System")]
    public GameObject orderBubblePrefab;

    private NPCOrder currentOrder;
    private GameObject currentBubble;
    private bool hasCreatedOrder = false;

    public float speed = 2f;
    public float rotationSpeed = 5f;
    public float stoppingDistance = 0.2f;

    private int currentIndex;
    private int direction = 1;

    private CharacterController controller;
    private Animator animator;

    private bool isInitialized = false;

    public float avoidRadius = 1.5f;
    public float avoidForce = 3f;

    private float gravity = -9.8f;
    private float yVelocity = 0;

    private Vector3 randomOffset;

    public System.Action OnDestroyCallback;

    private bool isOrderingDone = false;

    void OnDestroy()
    {
        OnDestroyCallback?.Invoke();
    }

    public void SetStart(bool fromStart)
    {
        isInitialized = true;

        if (fromStart)
        {
            direction = 1;
            currentIndex = 0;
        }
        else
        {
            direction = -1;
            currentIndex = path.Count() - 1;
        }
    }

    // ================= SHOP =================

    public void EnterShop(NPCQueueManager manager, Transform order, Transform exit)
    {
        queueManager = manager;
        orderPoint = order;
        exitPoint = exit;

        queueTarget = orderPoint;
        currentState = NPCState.GoingToQueue;
    }

    public void SetQueuePosition(Transform pos)
    {
        queueTarget = pos;
        currentState = NPCState.WaitingInQueue;
    }

    public void GoToExit(Transform exit)
    {
        exitPoint = exit;
        currentState = NPCState.LeavingShop;
    }

    public void CompleteOrder()
    {
        isOrderingDone = true;
    }

    // ================= INIT =================

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        randomOffset = new Vector3(
            Random.Range(-0.5f, 0.5f),
            0,
            Random.Range(-0.5f, 0.5f)
        );

        if (!isInitialized)
        {
            direction = (Random.value > 0.5f) ? 1 : -1;
            currentIndex = (direction == 1) ? 0 : path.Count() - 1;
        }
    }

    // ================= UPDATE =================

    void Update()
    {
        switch (currentState)
        {
            case NPCState.WalkingPath:
                HandlePathMovement();
                return;

            case NPCState.GoingToQueue:
            case NPCState.WaitingInQueue:

                if (queueTarget == null) return;

                float dist = Vector3.Distance(transform.position, queueTarget.position);

                if (dist < stoppingDistance)
                {
                    StopMoving();

                    if (queueManager != null && queueManager.IsFirst(this))
                    {
                        currentState = NPCState.Ordering;
                    }

                    return;
                }

                MoveTo(queueTarget.position);
                return;

            case NPCState.Ordering:

                // 👉 TẠO ORDER + UI (CHỈ 1 LẦN)
                if (!hasCreatedOrder)
                {
                    CreateOrder();
                    hasCreatedOrder = true;
                }

                // quay mặt về quầy
                if (orderPoint != null)
                {
                    Vector3 lookDir = orderPoint.position - transform.position;
                    lookDir.y = 0;

                    if (lookDir.sqrMagnitude > 0.01f)
                    {
                        Quaternion rot = Quaternion.LookRotation(lookDir);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);
                    }
                }

                StopMoving();

                if (isOrderingDone)
                {
                    if (queueManager != null)
                        queueManager.FinishOrder(this);

                    ClearOrder();
                    currentState = NPCState.LeavingShop;
                }
                return;

            case NPCState.LeavingShop:

                if (exitPoint == null) return;

                MoveTo(exitPoint.position);

                if (Vector3.Distance(transform.position, exitPoint.position) < stoppingDistance)
                {
                    StopMoving();
                    currentState = NPCState.WalkingPath;
                }
                return;
        }
    }

    // ================= ORDER =================

    void CreateOrder()
    {
        if (orderBubblePrefab == null) return;

        // 👉 tạo data riêng
        currentOrder = new NPCOrder();

        // 👉 tạo UI riêng
        currentBubble = Instantiate(orderBubblePrefab, transform);
        currentBubble.transform.localPosition = new Vector3(0, 3.5f, 0.8f);

        // 👉 setup UI
        OrderBubbleUI ui = currentBubble.GetComponent<OrderBubbleUI>();
        if (ui != null)
        {
            ui.Setup(currentOrder);
        }
    }

    void ClearOrder()
    {
        if (currentBubble != null)
        {
            Destroy(currentBubble);
        }

        currentOrder = null;
    }

    void StopMoving()
    {
        yVelocity = -2f;
        controller.Move(Vector3.zero);

        if (animator != null)
        {
            animator.SetFloat("Speed", 0);
        }
    }

    // ================= MOVE =================

    void MoveTo(Vector3 targetPos)
    {
        Vector3 dir = (targetPos - transform.position);
        dir.y = 0;

        Collider[] hits = Physics.OverlapSphere(transform.position, avoidRadius);
        Vector3 avoidDir = Vector3.zero;

        foreach (var hit in hits)
        {
            if (hit.gameObject != gameObject && hit.CompareTag("NPC"))
            {
                Vector3 diff = transform.position - hit.transform.position;
                float distance = diff.magnitude;

                if (distance > 0.01f)
                    avoidDir += diff.normalized / distance;
            }
        }

        Vector3 finalDir = (dir.normalized + avoidDir * avoidForce).normalized;

        if (controller.isGrounded && yVelocity < 0)
            yVelocity = -2f;

        yVelocity += gravity * Time.deltaTime;

        Vector3 move = finalDir * speed;
        move.y = yVelocity;

        controller.Move(move * Time.deltaTime);

        if (finalDir.sqrMagnitude > 0.01f)
        {
            Quaternion rot = Quaternion.LookRotation(finalDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
        }

        if (animator != null)
        {
            float actualSpeed = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;
            animator.SetFloat("Speed", actualSpeed);
        }
    }

    // ================= PATH =================

    void HandlePathMovement()
    {
        if (path == null || path.Count() == 0) return;

        Transform target = path.GetWaypoint(currentIndex);

        Vector3 targetPos = target.position + randomOffset;

        MoveTo(targetPos);

        Vector3 flatPos = transform.position;
        Vector3 flatTarget = targetPos;

        flatPos.y = 0;
        flatTarget.y = 0;

        if (Vector3.Distance(flatPos, flatTarget) < stoppingDistance)
        {
            currentIndex += direction;

            if (currentIndex >= path.Count() || currentIndex < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}