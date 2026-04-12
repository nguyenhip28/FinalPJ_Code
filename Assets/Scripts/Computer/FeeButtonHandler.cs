using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FeeButtonHandler : MonoBehaviour
{
    public int amount; // tiền của fee này (170, 300, 400)

    public Button payButton;
    public TextMeshProUGUI statusText;

    private bool isPaid = false;

    public void OnPayClicked()
    {
        if (isPaid) return;

        if (PlayerMoney.Instance.CanAfford(amount))
        {
            PlayerMoney.Instance.Spend(amount);

            isPaid = true;

            payButton.gameObject.SetActive(false);
            statusText.text = "Successful";
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }
}