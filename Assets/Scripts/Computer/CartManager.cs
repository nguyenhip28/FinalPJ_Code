using UnityEngine;
using TMPro;

public class CartManager : MonoBehaviour
{
    public static CartManager Instance;

    public ItemUI[] items;

    public TextMeshProUGUI totalText;
    public Transform spawnPoint;
    public GameObject[] itemPrefabs;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateTotal()
    {
        int total = 0;

        foreach (ItemUI item in items)
        {
            total += item.GetTotalPrice();
        }

        totalText.text = "Total: " + total.ToString();
    }
    public void Buy()
    {
        int total = 0;

        foreach (ItemUI item in items)
        {
            total += item.GetTotalPrice();
        }

        // check tiền
        if (!PlayerMoney.Instance.CanAfford(total))
        {
            Debug.Log("Not enough money!");
            return;
        }

        // trừ tiền
        PlayerMoney.Instance.Spend(total);

        // spawn item
        SpawnItems();

        // reset giỏ hàng
        foreach (ItemUI item in items)
        {
            item.ResetItem();
        }

        UpdateTotal();
    }
    void SpawnItems()
    {
        for (int i = 0; i < items.Length; i++)
        {
            int quantity = items[i].GetQuantity();

            for (int j = 0; j < quantity; j++)
            {
                Instantiate(itemPrefabs[i], spawnPoint.position, Quaternion.identity);
            }
        }
    }
}