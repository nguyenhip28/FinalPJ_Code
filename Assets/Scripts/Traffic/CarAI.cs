using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CarAI : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 8f;
    public float rotationSpeed = 6f;
    public float reachDistance = 2.5f;

    [Header("Detection - Vehicle")]
    public float detectDistance = 8f;
    public float stopDistance = 2.2f;
    public float slowDistance = 5f;
    public float detectRadius = 0.8f;
    public LayerMask vehicleLayer;

    [Header("Waypoint")]
    public Waypoint currentWaypoint;

    private float currentSpeed;
    private bool isWaitingForLight = false;

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
        {
            FindNearestWaypoint();
        }
    }

    void Update()
    {
        if (currentWaypoint == null) return;

        DetectTrafficLight(); // 🚦 check intersection
        DetectVehicle();      // 🚗 check xe
        Move();               // 🚀 di chuyển
    }

    // 🚗 MOVE
    void Move()
    {
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
    }

    // 🔀 NEXT WAYPOINT
    void ChooseNextWaypoint()
    {
        Waypoint next = currentWaypoint.GetNextWaypoint();

        if (next != null)
        {
            currentWaypoint = next;
        }
    }

    // 🚧 AVOID VEHICLE
    void DetectVehicle()
    {
        if (isWaitingForLight) return;

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

            if (dist <= stopDistance)
            {
                currentSpeed = 0f;
            }
            else if (dist <= slowDistance)
            {
                float t = (dist - stopDistance) / (slowDistance - stopDistance);
                float targetSpeed = Mathf.Lerp(0f, maxSpeed, t);

                currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * 6f);
            }
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, Time.deltaTime * 3f);
        }
    }

    // 🚦 CHECK INTERSECTION (KHÔNG CẦN RAYCAST)
    void DetectTrafficLight()
    {
        if (currentWaypoint == null) return;

        Waypoint next = currentWaypoint.GetNextWaypoint();
        if (next == null) return;

        if (next.type == Waypoint.WaypointType.Intersection && next.trafficLight != null)
        {
            TrafficLight light = next.trafficLight;

            float dist = Vector3.Distance(transform.position, next.transform.position);

            switch (light.currentState)
            {
                case TrafficLight.LightState.Red:
                    if (dist < 6f)
                    {
                        currentSpeed = 0f;
                        isWaitingForLight = true;
                        return;
                    }
                    break;

                case TrafficLight.LightState.Yellow:
                    currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed * 0.3f, Time.deltaTime * 5f);
                    return;

                case TrafficLight.LightState.Green:
                    isWaitingForLight = false;
                    break;
            }
        }

        if (isWaitingForLight)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, Time.deltaTime * 2f);
            isWaitingForLight = false;
        }
    }

    // 🔍 FIND NEAREST TRAFFIC LIGHT
    TrafficLight FindNearestTrafficLight(Vector3 position)
    {
        TrafficLight[] lights = FindObjectsOfType<TrafficLight>();

        float minDist = Mathf.Infinity;
        TrafficLight nearest = null;

        foreach (var light in lights)
        {
            float dist = Vector3.Distance(position, light.transform.position);

            if (dist < minDist)
            {
                minDist = dist;
                nearest = light;
            }
        }

        return nearest;
    }

    // 🔍 FIND WAYPOINT
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

    // 🧪 DEBUG
    void OnDrawGizmos()
    {
        Vector3 origin = transform.position + Vector3.up * 0.5f;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin + transform.forward * detectDistance, detectRadius);

        if (currentWaypoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, currentWaypoint.transform.position);
        }
    }
}