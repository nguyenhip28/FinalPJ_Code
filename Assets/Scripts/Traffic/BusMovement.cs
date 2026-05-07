using UnityEngine;

public class BusMovement : MonoBehaviour
{
    public Waypoint currentWaypoint;
    public float speed = 5f;
    public float reachDistance = 0.5f;

    [Header("Destroy Settings")]
    public Waypoint destroyAtWaypoint; 

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

        transform.position += direction * speed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.deltaTime);
        }

        float distance = Vector3.Distance(transform.position, targetWaypoint.transform.position);

        if (distance < reachDistance)
        {
            ArriveAtWaypoint();
        }
    }

    void ArriveAtWaypoint()
    {

        if (targetWaypoint == destroyAtWaypoint)
        {
            Destroy(gameObject);
            return;
        }


        if (targetWaypoint.isStopPoint)
        {
        }

        currentWaypoint = targetWaypoint;
        targetWaypoint = currentWaypoint.GetNextWaypoint();
    }
}