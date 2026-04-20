using UnityEngine;

public class FoodBox : MonoBehaviour
{
    public BoxType boxType;

    public GameObject foodPrefab;
    public int maxAmount = 10;

    private int currentAmount;

    void Awake()
    {
        currentAmount = maxAmount;
    }

    public BoxData GetData()
    {
        BoxData data = new BoxData();

        data.boxType = (int)boxType;
        data.amount = currentAmount;

        data.posX = transform.position.x;
        data.posY = transform.position.y;
        data.posZ = transform.position.z;

        data.rotY = transform.eulerAngles.y;

        return data;
    }

    public void LoadFromData(BoxData data)
    {
        currentAmount = data.amount;

        transform.position = new Vector3(data.posX, data.posY, data.posZ);
        transform.rotation = Quaternion.Euler(0, data.rotY, 0);
    }

    public bool HasFood()
    {
        return currentAmount > 0;
    }

    public void UseOne()
    {
        if (currentAmount <= 0)
        {
            Debug.Log("Box hết đồ!");
            return;
        }

        currentAmount--;
    }
}