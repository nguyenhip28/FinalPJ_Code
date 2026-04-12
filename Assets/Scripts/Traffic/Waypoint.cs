using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [Header("Connections")]
    public List<Waypoint> nextPoints = new List<Waypoint>();

    public enum WaypointType
    {
        Normal,
        Intersection
    }

    public WaypointType type = WaypointType.Normal;

    [Header("Traffic Light")]
    public TrafficLight trafficLight; // 🔥 thêm dòng này
    
    public bool isStopPoint = false;

    public Waypoint GetNextWaypoint()
    {
        if (nextPoints == null || nextPoints.Count == 0)
            return null;

        if (type == WaypointType.Intersection)
        {
            return nextPoints[Random.Range(0, nextPoints.Count)];
        }

        return nextPoints[0];
    }

    void OnDrawGizmos()
    {
        Gizmos.color = (type == WaypointType.Intersection) ? Color.yellow : Color.green;

        foreach (var wp in nextPoints)
        {
            if (wp != null)
            {
                Gizmos.DrawLine(transform.position, wp.transform.position);
            }
        }
    }
}