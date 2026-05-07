using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FeeButtonHandler : MonoBehaviour
{
    public int amount; 

    public Button payButton;
    public TextMeshProUGUI statusText;

    public int feeIndex; 

    private bool isPaid = false;
    public void OnPayClicked()
    {
        if (isPaid) return;

        var fee = FeeManager.Instance.fees[feeIndex];
        bool success = FeeManager.Instance.PayFee(fee);

        if (success)
        {
            isPaid = true;
            payButton.gameObject.SetActive(false);
            statusText.text = "Successful";
        }
    }
}