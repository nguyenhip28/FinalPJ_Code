using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Transform target; // player (điểm gốc)
    public float maxDistance = 0.4f; // khoảng cách cam
    public float minDistance = 0.1f;
    public LayerMask collisionMask;

    void LateUpdate()
    {
        Vector3 dir = (transform.localPosition).normalized;

        RaycastHit hit;
        if (Physics.Raycast(target.position, target.forward, out hit, maxDistance, collisionMask))
        {
            float dist = hit.distance - 0.05f;
            dist = Mathf.Clamp(dist, minDistance, maxDistance);
            transform.localPosition = new Vector3(0, 0, dist);
        }
        else
        {
            transform.localPosition = new Vector3(0, 0, maxDistance);
        }
    }
}