using UnityEngine;
using TMPro;

public class PlayerInteractionView : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI hintText;

    [Header("Knife Visual")]
    public GameObject knifeVisual;

    void Start()
    {
        if (knifeVisual != null)
            knifeVisual.SetActive(false);
    }

    public void ShowHint(string text)
    {
        if (hintText == null) return;

        hintText.gameObject.SetActive(true);
        hintText.text = text;
    }

    public void HideHint()
    {
        if (hintText == null) return;

        hintText.gameObject.SetActive(false);
    }

    public void ShowKnife(bool show)
    {
        if (knifeVisual != null)
            knifeVisual.SetActive(show);
    }
}