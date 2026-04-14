using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public Transform[] waypoints;

    void Awake()
    {
        // Lấy tất cả WP_NPC con của object này
        waypoints = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }
    }

    public Transform GetWaypoint(int index)
    {
        return waypoints[index];
    }

    public int Count()
    {
        return waypoints.Length;
    }
}