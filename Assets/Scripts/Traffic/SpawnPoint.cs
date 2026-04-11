using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public int maxCars = 3;
    public List<GameObject> currentCars = new List<GameObject>();

    public bool CanSpawn()
    {
        currentCars.RemoveAll(car => car == null);
        return currentCars.Count < maxCars;
    }

    public void RegisterCar(GameObject car)
    {
        currentCars.Add(car);
    }
}