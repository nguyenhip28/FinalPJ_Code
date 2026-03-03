using UnityEngine;
using UnityEngine.UI;

public class CookingProgressUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    [Header("Colors")]
    [SerializeField] private Color cookingColor = Color.green;
    [SerializeField] private Color warningColor = Color.yellow;
    [SerializeField] private Color burnColor = Color.red;

    public void SetProgress(float value)
    {
        value = Mathf.Clamp01(value);
        fillImage.fillAmount = value;

        // Đổi màu theo progress
        if (value < 0.5f)
        {
            fillImage.color = cookingColor;
        }
        else if (value < 0.8f)
        {
            fillImage.color = warningColor;
        }
        else
        {
            fillImage.color = burnColor;
        }
    }
}