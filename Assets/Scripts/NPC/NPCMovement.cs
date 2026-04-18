using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class NPCMovement : MonoBehaviour
{
    public WaypointManager path;

    public float speed = 2f;
    public float rotationSpeed = 5f;
    public float stoppingDistance = 0.2f;

    private int currentIndex;
    private int direction = 1;

    private CharacterController controller;
    private Animator animator;

    private bool isInitialized = false;

    // Avoidance
    public float avoidRadius = 1.5f;
    public float avoidForce = 3f;

    // Gravity
    private float gravity = -9.8f;
    private float yVelocity = 0;

    // Offset tránh đâm nhau
    private Vector3 randomOffset;

    public System.Action OnDestroyCallback;

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

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // random offset mỗi NPC
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

    void Update()
    {
        if (path == null || path.Count() == 0) return;

        Transform target = path.GetWaypoint(currentIndex);

        // offset tránh đâm nhau
        Vector3 targetPos = target.position + randomOffset;

        Vector3 dir = (targetPos - transform.position);
        dir.y = 0;

        // ===== AVOID NPC =====
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

        // ===== GRAVITY =====
        if (controller.isGrounded && yVelocity < 0)
        {
            yVelocity = -2f;
        }

        yVelocity += gravity * Time.deltaTime;

        Vector3 move = finalDir * speed;
        move.y = yVelocity;

        controller.Move(move * Time.deltaTime);

        // ===== ROTATE =====
        if (finalDir.sqrMagnitude > 0.01f)
        {
            Quaternion rot = Quaternion.LookRotation(finalDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
        }

        // ===== ANIMATION =====
        if (animator != null)
        {
            float actualSpeed = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;
            animator.SetFloat("Speed", actualSpeed);
        }

        // ===== NEXT WAYPOINT =====
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