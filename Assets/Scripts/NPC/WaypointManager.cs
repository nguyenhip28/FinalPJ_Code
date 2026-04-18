using UnityEngine;
using System.Linq;

public class WaypointManager : MonoBehaviour
{
    public Transform[] waypoints;

    void Awake()
    {
        waypoints = GetComponentsInChildren<Transform>()
            .Where(t => t != transform)
            .OrderBy(t => t.name)
            .ToArray();
    }

    public Transform GetWaypoint(int index)
    {
        if (index < 0 || index >= waypoints.Length)
            return null;

        return waypoints[index];
    }

    public int Count()
    {
        return waypoints.Length;
    }

    void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Gizmos.color = Color.green;

        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            if (waypoints[i] != null && waypoints[i + 1] != null)
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
    }
}