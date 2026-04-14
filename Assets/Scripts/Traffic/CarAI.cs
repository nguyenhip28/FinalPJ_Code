using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CarAI : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 8f;
    public float rotationSpeed = 6f;
    public float reachDistance = 2.5f;

    [Header("Detection")]
    public float detectDistance = 10f;
    public float stopDistance = 3.5f;
    public float slowDistance = 7f;
    public float detectRadius = 0.8f;
    public LayerMask vehicleLayer;

    [Header("Waypoint")]
    public Waypoint currentWaypoint;

    private float currentSpeed;

    // 🚧 trạng thái
    private bool isWaitingAtLight = false;
    private bool isYielding = false;

    private bool hasDecision = false;
    private int priority; // số random

    void Start()
    {
        currentSpeed = maxSpeed;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        if (currentWaypoint == null)
            FindNearestWaypoint();
    }

    void Update()
    {
        if (currentWaypoint == null) return;

        HandleTrafficLight();
        DetectVehicle();
        Move();
    }

    // =========================
    // 🚦 TRAFFIC LIGHT
    // =========================
    void HandleTrafficLight()
    {
        if (!currentWaypoint.isStopPoint)
        {
            isWaitingAtLight = false;
            return;
        }

        if (currentWaypoint.trafficLight == null) return;

        var state = currentWaypoint.trafficLight.currentState;

        float dist = Vector3.Distance(transform.position, currentWaypoint.transform.position);

        if (state == TrafficLight.LightState.Red && dist < 3f)
        {
            isWaitingAtLight = true;
            currentSpeed = 0;
        }
        else if (state == TrafficLight.LightState.Yellow && dist < 5f)
        {
            isWaitingAtLight = false;
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed * 0.4f, Time.deltaTime * 5f);
        }
        else
        {
            isWaitingAtLight = false;
        }
    }

    // =========================
    // 🚗 AVOID VEHICLE + YIELD
    // =========================
    void DetectVehicle()
    {
        if (isWaitingAtLight) return;

        RaycastHit hit;

        Vector3 origin = transform.position + Vector3.up * 0.5f;

        if (Physics.SphereCast(origin,
                               detectRadius,
                               transform.forward,
                               out hit,
                               detectDistance,
                               vehicleLayer))
        {
            if (hit.collider.gameObject == gameObject) return;

            float dist = hit.distance;

            CarAI otherCar = hit.collider.GetComponent<CarAI>();

            // =========================
            // 🚧 INTERSECTION YIELD
            // =========================
            if (IsInIntersection() && otherCar != null)
            {
                // 👉 mỗi xe chỉ random 1 lần khi vào ngã tư
                if (!hasDecision)
                {
                    priority = Random.Range(0, 100);
                    hasDecision = true;
                }

                // đảm bảo xe kia cũng có priority
                if (!otherCar.hasDecision)
                {
                    otherCar.priority = Random.Range(0, 100);
                    otherCar.hasDecision = true;
                }

                // 👉 so sánh quyền ưu tiên
                if (priority < otherCar.priority)
                {
                    currentSpeed = Mathf.Lerp(currentSpeed, 0f, Time.deltaTime * 6f);
                    return;
                }
            }

            // =========================
            // 🚗 ADVANCED FOLLOW (FIX DÍNH XE)
            // =========================

            // 🔥 trừ chiều dài xe (rất quan trọng)
            dist -= 1.5f;

            // khoảng cách an toàn
            float safeGap = 4f;

            // tính speed theo khoảng cách (mượt)
            float ratio = dist / detectDistance;
            float targetSpeed = maxSpeed * ratio;

            // clamp lại
            targetSpeed = Mathf.Clamp(targetSpeed, 0f, maxSpeed);

            // ❌ quá gần → dừng hẳn
            if (dist < safeGap)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, 0f, Time.deltaTime * 8f);
            }
            else
            {
                // ✅ chạy mượt theo xe trước
                currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * 4f);
            }
        }
        else
        {
            isYielding = false;
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, Time.deltaTime * 3f);
        }
    }

    // =========================
    // 🚗 MOVE
    // =========================
    void Move()
    {
        if (isWaitingAtLight) return;

        Transform wp = currentWaypoint.transform;

        Vector3 direction = (wp.position - transform.position).normalized;

        transform.position += direction * currentSpeed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, wp.position) < reachDistance)
        {
            ChooseNextWaypoint();
        }

        if (!IsInIntersection())
        {
            hasDecision = false;
        }
    }

    void ChooseNextWaypoint()
    {
        Waypoint next = currentWaypoint.GetNextWaypoint();
        if (next != null)
            currentWaypoint = next;
    }

    bool IsInIntersection()
    {
        return currentWaypoint.type == Waypoint.WaypointType.Intersection;
    }

    void FindNearestWaypoint()
    {
        Waypoint[] all = FindObjectsOfType<Waypoint>();

        float min = Mathf.Infinity;
        Waypoint nearest = null;

        foreach (var wp in all)
        {
            float d = Vector3.Distance(transform.position, wp.transform.position);
            if (d < min)
            {
                min = d;
                nearest = wp;
            }
        }

        currentWaypoint = nearest;
    }
}