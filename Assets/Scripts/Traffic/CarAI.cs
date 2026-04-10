using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CarAI : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 8f;
    public float rotationSpeed = 6f;
    public float reachDistance = 2.5f;

    [Header("Detection")]
    public float detectDistance = 5f;
    public LayerMask vehicleLayer;

    [Header("Waypoint")]
    public Waypoint currentWaypoint;

    private bool isStopped = false;

    void Start()
    {
        // ✅ Fix vật lý
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        // ✅ Auto tìm waypoint gần nhất nếu chưa gán
        if (currentWaypoint == null)
        {
            FindNearestWaypoint();
        }
    }

    void Update()
    {
        if (currentWaypoint == null) return;

        DetectVehicle();
        Move();
    }

    // 🚗 DI CHUYỂN CHUẨN THEO WAYPOINT
    void Move()
    {
        if (isStopped) return;

        Transform wpTransform = currentWaypoint.transform;

        // ✅ HƯỚNG ĐÚNG: từ xe → waypoint
        Vector3 direction = (wpTransform.position - transform.position).normalized;

        // 👉 di chuyển
        transform.position += direction * speed * Time.deltaTime;

        // 👉 xoay mượt theo hướng di chuyển
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // 👉 kiểm tra tới waypoint
        float distance = Vector3.Distance(transform.position, wpTransform.position);
        if (distance < reachDistance)
        {
            ChooseNextWaypoint();
        }
    }

    // 🔀 CHỌN WAYPOINT TIẾP THEO
    void ChooseNextWaypoint()
    {
        if (currentWaypoint == null) return;

        Waypoint next = currentWaypoint.GetNextWaypoint();

        if (next == null)
        {
            Debug.LogWarning("Waypoint không có next!", currentWaypoint);
            return;
        }

        currentWaypoint = next;

        // 🧪 Debug
        // Debug.Log("Next WP: " + currentWaypoint.name);
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

    // 🔍 AUTO TÌM WAYPOINT GẦN NHẤT
    void FindNearestWaypoint()
    {
        Waypoint[] allWaypoints = FindObjectsOfType<Waypoint>();

        float minDist = Mathf.Infinity;
        Waypoint nearest = null;

        foreach (Waypoint wp in allWaypoints)
        {
            float dist = Vector3.Distance(transform.position, wp.transform.position);

            if (dist < minDist)
            {
                minDist = dist;
                nearest = wp;
            }
        }

        currentWaypoint = nearest;

        if (nearest != null)
        {
            Debug.Log("Auto assigned waypoint: " + nearest.name);
        }
        else
        {
            Debug.LogError("Không tìm thấy waypoint nào trong scene!");
        }
    }

    // 🧪 DEBUG
    void OnDrawGizmos()
    {
        // 🔴 Ray phía trước
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up * 0.5f,
                        transform.position + Vector3.up * 0.5f + transform.forward * detectDistance);

        // 🔵 Line tới waypoint hiện tại
        if (currentWaypoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, currentWaypoint.transform.position);
        }
    }
}