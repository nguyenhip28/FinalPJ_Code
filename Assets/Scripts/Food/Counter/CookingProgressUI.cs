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
        currentFood = food;

        if (currentFood != null)
        {
            currentFood.OnCookingProgress += UpdateBar;

            fillImage.fillAmount = 0f;

            gameObject.SetActive(true);
        }
    }

    void UpdateBar(float progress)
    {
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
            transform.forward = Camera.main.transform.forward;
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