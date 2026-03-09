using UnityEngine;
using UnityEngine.UI;

public class CookingProgressUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    private FoodItem currentFood;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetFood(FoodItem food)
    {
        if (currentFood != null)
        {
            currentFood.OnCookingProgress -= UpdateBar;
        }

        currentFood = food;

        if (currentFood != null)
        {
            currentFood.OnCookingProgress += UpdateBar;

            if (fillImage != null)
                fillImage.fillAmount = 0f;

            gameObject.SetActive(true);
        }
    }

    void UpdateBar(float progress)
    {
        if (fillImage == null) return;

        fillImage.fillAmount = progress;

        if (progress >= 1f)
        {
            gameObject.SetActive(false);
        }
    }

    void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180, 0);
        }
    }

    void OnDestroy()
    {
        if (currentFood != null)
        {
            currentFood.OnCookingProgress -= UpdateBar;
        }
    }
}