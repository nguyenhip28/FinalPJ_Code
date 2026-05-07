using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FeeManager : MonoBehaviour
{
    public static FeeManager Instance;

    public List<FeeData> fees = new List<FeeData>();

    private bool hasEnded = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
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

        if (PlayerMoney.Instance.CanAfford(fee.amount))
        {
            PlayerMoney.Instance.Spend(fee.amount);
            fee.isPaid = true;

            Debug.Log(fee.feeName + " paid!");

            CheckAllFeesPaid();

            return true;
        }

        Debug.Log("Not enough money!");
        return false;
    }

    void CheckAllFeesPaid()
    {
        foreach (var fee in fees)
        {
            if (!fee.isPaid)
                return; 
        }

        if (!hasEnded)
        {
            hasEnded = true;
            TriggerEnding();
        }
    }

    void TriggerEnding()
    {
        Debug.Log("GAME COMPLETE!");
        SceneManager.LoadScene("EndingScene");
    }
}