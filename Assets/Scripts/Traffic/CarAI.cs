using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CarAI : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 8f;
    public float rotationSpeed = 6f;
    public float reachDistance = 1.5f;

    [Header("Detection")]
    public float detectDistance = 5f;
    public LayerMask vehicleLayer;

    [Header("Waypoint")]
    public Waypoint currentWaypoint;

    private bool isStopped = false;

    void Start()
    {
        // 👉 Đảm bảo xe không bị gravity kéo xuống
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    void Update()
    {
        if (currentWaypoint == null) return;

        DetectVehicle();
        Move();
    }

    void Move()
    {
        if (isStopped) return;

        Vector3 targetPos = currentWaypoint.transform.position;

        // ✅ hướng chuẩn: từ xe → waypoint
        Vector3 direction = (targetPos - transform.position).normalized;

        // 👉 di chuyển
        transform.position += direction * speed * Time.deltaTime;

        // 👉 xoay mượt theo hướng di chuyển
        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        // 👉 tới waypoint
        float distance = Vector3.Distance(transform.position, targetPos);
        if (distance < reachDistance)
        {
            ChooseNextWaypoint();
        }
    }

    // 🔀 CHỌN WAYPOINT TIẾP THEO
    void ChooseNextWaypoint()
    {
        if (currentWaypoint.nextPoints == null || currentWaypoint.nextPoints.Count == 0)
            return;

        if (currentWaypoint.type == Waypoint.WaypointType.Intersection)
        {
            // 👉 random rẽ trái / phải / thẳng
            int rand = Random.Range(0, currentWaypoint.nextPoints.Count);
            currentWaypoint = currentWaypoint.nextPoints[rand];
        }
        else
        {
            // 👉 lane thường chỉ có 1 hướng
            currentWaypoint = currentWaypoint.nextPoints[0];
        }
    }

    // 🚧 PHÁT HIỆN XE PHÍA TRƯỚC
    void DetectVehicle()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, detectDistance, vehicleLayer))
        {
            isStopped = true;
        }
        else
        {
            isStopped = false;
        }
    }

    // 🧪 DEBUG
    void OnDrawGizmos()
    {
        // Ray phía trước
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up * 0.5f,
                        transform.position + Vector3.up * 0.5f + transform.forward * detectDistance);

        // waypoint hiện tại
        if (currentWaypoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, currentWaypoint.transform.position);
        }
    }
}