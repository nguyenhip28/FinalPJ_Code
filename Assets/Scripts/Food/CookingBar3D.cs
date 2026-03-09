using UnityEngine;

public class CookingBar3D : MonoBehaviour
{
    [SerializeField] private Transform fill;

    private FoodItem food;

    public void SetFood(FoodItem f)
    {
        food = f;
        food.OnCookingProgress += UpdateBar;
    }

    void UpdateBar(float progress)
    {
        fill.localScale = new Vector3(progress, 1, 1);
        fill.localPosition = new Vector3(progress / 2 - 0.5f, 0, 0);
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
        if (food != null)
        {
            food.OnCookingProgress -= UpdateBar;
        }
    }
}