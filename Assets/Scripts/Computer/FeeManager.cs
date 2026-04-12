using System.Collections.Generic;
using UnityEngine;

public class FeeManager : MonoBehaviour
{
    public static FeeManager Instance;

    public List<FeeData> fees = new List<FeeData>();

    public int currentDay = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        fees.Add(new FeeData("Electricity", 170));
        fees.Add(new FeeData("Rent", 300));
        fees.Add(new FeeData("Debt", 400));
    }

    public bool PayFee(FeeData fee)
    {
        if (fee.isPaid) return false;

        // 🔥 dùng PlayerMoney của bạn
        if (PlayerMoney.Instance.CanAfford(fee.amount))
        {
            PlayerMoney.Instance.Spend(fee.amount);
            fee.isPaid = true;
            return true;
        }

        Debug.Log("Not enough money!");
        return false;
    }

    public void NextDay()
    {
        currentDay++;

        if (currentDay % 7 == 0)
        {
            ResetFees();
        }
    }

    void ResetFees()
    {
        foreach (var fee in fees)
        {
            fee.isPaid = false;
        }
    }
}