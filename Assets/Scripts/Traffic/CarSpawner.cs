using UnityEngine;
using System.Collections.Generic;

public class CarSpawner : MonoBehaviour
{
    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    public List<GameObject> carPrefabs = new List<GameObject>();

    public float spawnInterval = 3f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnCar), 1f, spawnInterval);
    }

    void SpawnCar()
    {
        if (spawnPoints.Count == 0 || carPrefabs.Count == 0) return;

        SpawnPoint sp = spawnPoints[Random.Range(0, spawnPoints.Count)];

        if (!sp.CanSpawn()) return;

        GameObject prefab = carPrefabs[Random.Range(0, carPrefabs.Count)];

        Waypoint wp = sp.GetComponent<Waypoint>();
        if (wp == null || wp.nextPoints.Count == 0) return;

        // 🎯 LẤY HƯỚNG ĐẾN WAYPOINT TIẾP THEO
        Vector3 dir = (wp.nextPoints[0].transform.position - sp.transform.position).normalized;

        Quaternion rot = Quaternion.LookRotation(dir);

        GameObject car = Instantiate(prefab, sp.transform.position, rot);

        CarAI ai = car.GetComponent<CarAI>();
        if (ai != null)
        {
            ai.currentWaypoint = wp;
        }

        sp.RegisterCar(car);
    }
}