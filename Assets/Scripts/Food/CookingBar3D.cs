using UnityEngine;

public class CookingBar3D : MonoBehaviour
{
    [SerializeField] private Renderer barRenderer;

    [Header("Colors")]
    [SerializeField] private Color rawColor = Color.white;
    [SerializeField] private Color midColor = Color.yellow;
    [SerializeField] private Color cookedColor = Color.green;
    [SerializeField] private Color burnedColor = Color.red;

    private FoodItem food;

    public void SetFood(FoodItem f)
    {
        food = f;
        food.OnCookingProgress += UpdateBar;
    }

    void UpdateBar(float progress)
    {
        if (food == null) return;

        // 🔥 Chia mốc theo thời gian
        if (progress < 0.3f)
        {
            SetColor(rawColor);
        }
        else if (progress < 0.6f)
        {
            SetColor(midColor);
        }
        else if (progress < 1f)
        {
            SetColor(cookedColor);
        }
        else
        {
            SetColor(burnedColor);
        }
    }

    void SetColor(Color color)
    {
        barRenderer.material.color = color;
    }

    void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        if (food != null)
        {
            food.OnCookingProgress -= UpdateBar;
        }
    }
}