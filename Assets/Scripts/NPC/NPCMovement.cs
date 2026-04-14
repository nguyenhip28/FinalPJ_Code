using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class NPCMovement : MonoBehaviour
{
    public WaypointManager path;

    public float speed = 2f;
    public float rotationSpeed = 5f;
    public float stoppingDistance = 0.3f;

    private int currentIndex;
    private int direction = 1;

    private CharacterController controller;
    private Animator animator; // 🎬 Animation

    private bool isInitialized = false;

    // Avoidance
    public float avoidRadius = 1.5f;
    public float avoidForce = 2f;

    // Gravity
    private float gravity = -9.8f;
    private float yVelocity = 0;

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
        animator = GetComponent<Animator>(); // 🔥 LẤY ANIMATOR

        if (!isInitialized)
        {
            direction = (Random.value > 0.5f) ? 1 : -1;

            if (direction == 1)
                currentIndex = 0;
            else
                currentIndex = path.Count() - 1;
        }
    }

    void Update()
    {
        if (path == null || path.Count() == 0) return;

        Transform target = path.GetWaypoint(currentIndex);

        Vector3 dir = (target.position - transform.position);
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

                if (distance > 0)
                    avoidDir += diff.normalized / distance;
            }
        }

        Vector3 finalDir = (dir.normalized + avoidDir * avoidForce).normalized;

        // ===== GRAVITY =====
        if (controller.isGrounded && yVelocity < 0)
        {
            yVelocity = -2f; // giữ chân chạm đất
        }

        yVelocity += gravity * Time.deltaTime;

        Vector3 move = finalDir * speed;
        move.y = yVelocity;

        controller.Move(move * Time.deltaTime);

        // ===== ROTATE =====
        if (finalDir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(finalDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
        }

        // ===== 🎬 ANIMATION =====
        float moveAmount = new Vector3(move.x, 0, move.z).magnitude;

        if (animator != null)
        {
            animator.SetFloat("Speed", moveAmount);
        }

        // ===== NEXT WAYPOINT =====
        if (Vector3.Distance(transform.position, target.position) < stoppingDistance)
        {
            currentIndex += direction;

            if (currentIndex >= path.Count() || currentIndex < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}