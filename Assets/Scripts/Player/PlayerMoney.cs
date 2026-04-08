using UnityEngine;
using TMPro;

public class PlayerMoney : MonoBehaviour
{
    public static PlayerMoney Instance;

    public int money = 400;
    public TextMeshProUGUI moneyText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateUI(); // đảm bảo UI luôn đúng khi vào game
    }

    public void Spend(int amount)
    {
        money -= amount;
        if (money < 0) money = 0;
        UpdateUI();
    }

    public void Add(int amount)
    {
        money += amount;
        UpdateUI();
    }

    // 🔥 QUAN TRỌNG (GameManager sẽ gọi)
    public void SetMoney(int value)
    {
        money = value;
        UpdateUI();
    }

    public bool CanAfford(int amount)
    {
        return money >= amount;
    }

    void UpdateUI()
    {
        if (moneyText != null)
            moneyText.text = "Money: " + money + "$";
    }
}