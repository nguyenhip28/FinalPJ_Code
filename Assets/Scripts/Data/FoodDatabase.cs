using UnityEngine;
using System.Collections.Generic;

public class FoodDatabase : MonoBehaviour
{
    public static FoodDatabase Instance;

    public List<FoodItem> prefabs;

    private Dictionary<FoodType, GameObject> dict;

    void Awake()
    {
        Instance = this;

        dict = new Dictionary<FoodType, GameObject>();

        foreach (var p in prefabs)
        {
            dict[p.foodType] = p.gameObject;
        }
    }

    public GameObject GetPrefab(FoodType type)
    {
        return dict[type];
    }
}