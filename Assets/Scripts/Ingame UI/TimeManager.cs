using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    public float realDayDuration = 900f; // 15 phút = 900 giây
    private float gameTime = 7f; // bắt đầu 7h
    private int day = 1;

    void Update()
    {
        float timeSpeed = (21f - 7f) / realDayDuration;
        gameTime += Time.deltaTime * timeSpeed;

        if (gameTime >= 21f)
        {
            gameTime = 21f; // dừng tại 21h
        }

        UpdateUI();
        UpdateLighting();
    }

    void UpdateUI()
    {
        int hour = Mathf.FloorToInt(gameTime);
        int minute = Mathf.FloorToInt((gameTime - hour) * 60);

        timeText.text = $"Day {day} - {hour:00}:{minute:00}";
    }

    void UpdateLighting()
    {
        if (gameTime >= 7f && gameTime < 16f)
        {
            RenderSettings.ambientLight = Color.white;
        }
        else if (gameTime >= 16f && gameTime < 18f)
        {
            RenderSettings.ambientLight = Color.Lerp(Color.white, new Color(1f, 0.5f, 0.5f), (gameTime - 16f) / 2f);
        }
        else if (gameTime >= 18f)
        {
            RenderSettings.ambientLight = Color.Lerp(new Color(1f, 0.5f, 0.5f), Color.black, (gameTime - 18f) / 3f);
        }
    }
}