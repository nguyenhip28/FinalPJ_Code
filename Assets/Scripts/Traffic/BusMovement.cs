using UnityEngine;

public class BusMovement : MonoBehaviour
{
    public Waypoint currentWaypoint;
    public float speed = 5f;
    public float reachDistance = 0.5f;

    [Header("Destroy Settings")]
    public Waypoint destroyAtWaypoint; // 👉 waypoint sẽ destroy

    private Waypoint targetWaypoint;

    void Start()
    {
        if (currentWaypoint != null)
        {
            targetWaypoint = currentWaypoint.GetNextWaypoint();
        }
    }

    void Update()
    {
        if (targetWaypoint == null) return;

        MoveToWaypoint();
    }

    void MoveToWaypoint()
    {
        Vector3 direction = (targetWaypoint.transform.position - transform.position).normalized;

        // 👉 di chuyển
        transform.position += direction * speed * Time.deltaTime;

        // 👉 xoay theo hướng di chuyển
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.deltaTime);
        }

        // 👉 kiểm tra đã tới waypoint chưa
        float distance = Vector3.Distance(transform.position, targetWaypoint.transform.position);

        if (distance < reachDistance)
        {
            ArriveAtWaypoint();
        }
    }

    void ArriveAtWaypoint()
    {
        // 👉 nếu đây là waypoint cần destroy
        if (targetWaypoint == destroyAtWaypoint)
        {
            Destroy(gameObject);
            return;
        }

        // 👉 nếu là stop point (có thể mở rộng logic dừng)
        if (targetWaypoint.isStopPoint)
        {
            // TODO: thêm delay nếu muốn
            // StartCoroutine(WaitAtStop());
        }

        // 👉 chuyển waypoint tiếp theo
        currentWaypoint = targetWaypoint;
        targetWaypoint = currentWaypoint.GetNextWaypoint();
    }
}