using UnityEngine;

public class UITabManager : MonoBehaviour
{
    public GameObject orderPanel;
    public GameObject feePanel;
    public GameObject footer;

    public void OpenOrder()
    {
        orderPanel.SetActive(true);
        feePanel.SetActive(false);

        footer.SetActive(true);
    }

    public void OpenFee()
    {
        orderPanel.SetActive(false);
        feePanel.SetActive(true);

        footer.SetActive(false);

        Debug.Log("Fee opened → Footer hidden");
    }
}