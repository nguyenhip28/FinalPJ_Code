using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public List<Waypoint> nextPoints = new List<Waypoint>();

    public enum WaypointType
    {
        Normal,
        Intersection
    }

    public WaypointType type = WaypointType.Normal;

    public Waypoint GetNextWaypoint()
    {
        if (nextPoints.Count == 0) return null;
        return nextPoints[Random.Range(0, nextPoints.Count)];
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        foreach (var wp in nextPoints)
        {
            if (wp != null)
            {
                Gizmos.DrawLine(transform.position, wp.transform.position);
            }
        }
    }
}