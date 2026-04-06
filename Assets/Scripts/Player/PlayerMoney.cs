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
        UpdateUI();
    }

    public void Spend(int amount)
    {
        money -= amount;
        if (money < 0) money = 0;
        UpdateUI();
    }

    public bool CanAfford(int amount)
    {
        return money >= amount;
    }

    void UpdateUI()
    {
        moneyText.text = "Money: " + money.ToString() + "$";
    }
}