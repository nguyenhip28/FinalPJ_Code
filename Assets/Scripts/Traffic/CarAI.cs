using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CarAI : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 8f;
    public float rotationSpeed = 6f;
    public float reachDistance = 2.5f;

    [Header("Vehicle Detection")]
    public float detectDistance = 8f;
    public float stopDistance = 2.2f;
    public float slowDistance = 5f;
    public float detectRadius = 0.8f;
    public LayerMask vehicleLayer;

    [Header("Traffic Light")]
    public float lightDetectDistance = 6f;
    public LayerMask trafficLightLayer;

    [Header("Waypoint")]
    public Waypoint currentWaypoint;

    private float currentSpeed;

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

        DetectVehicle();
        DetectTrafficLight();
        Move();
    }

    void Move()
    {
        Vector3 dir = (currentWaypoint.transform.position - transform.position).normalized;

        transform.position += dir * currentSpeed * Time.deltaTime;

        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, currentWaypoint.transform.position) < reachDistance)
        {
            ChooseNextWaypoint();
        }
    }

    void ChooseNextWaypoint()
    {
        Waypoint next = currentWaypoint.GetNextWaypoint();
        if (next != null) currentWaypoint = next;
    }

    // 🚧 tránh xe
    void DetectVehicle()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position + Vector3.up * 0.5f,
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
                float target = Mathf.Lerp(0f, maxSpeed, t);
                currentSpeed = Mathf.Lerp(currentSpeed, target, Time.deltaTime * 6f);
            }
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, Time.deltaTime * 3f);
        }
    }

    // 🚦 detect đèn
    void DetectTrafficLight()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.up * 0.5f,
                            transform.forward,
                            out hit,
                            lightDetectDistance,
                            trafficLightLayer))
        {
            TrafficLight light = hit.collider.GetComponent<TrafficLight>();

            if (light != null)
            {
                if (light.currentState == TrafficLight.LightState.Red)
                {
                    currentSpeed = 0f;
                }
                else if (light.currentState == TrafficLight.LightState.Yellow)
                {
                    currentSpeed = Mathf.Lerp(currentSpeed, 2f, Time.deltaTime * 5f);
                }
            }
        }
    }

    void FindNearestWaypoint()
    {
        Waypoint[] all = FindObjectsOfType<Waypoint>();

        float min = Mathf.Infinity;
        foreach (var wp in all)
        {
            float d = Vector3.Distance(transform.position, wp.transform.position);
            if (d < min)
            {
                min = d;
                currentWaypoint = wp;
            }
        }
    }
}