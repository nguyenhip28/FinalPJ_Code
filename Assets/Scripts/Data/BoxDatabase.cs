using UnityEngine;
using System.Collections.Generic;

public class BoxDatabase : MonoBehaviour
{
    public static BoxDatabase Instance;

    public List<FoodBox> prefabs;

    private Dictionary<BoxType, GameObject> dict;

    void Awake()
    {
        Instance = this;

        dict = new Dictionary<BoxType, GameObject>();

        foreach (var p in prefabs)
        {
            dict[p.boxType] = p.gameObject;
        }
    }

    public GameObject GetPrefab(BoxType type)
    {
        return dict[type];
    }
}