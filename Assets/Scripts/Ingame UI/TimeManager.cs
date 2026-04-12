using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI timeText;

    [Header("Time Settings")]
    public float realDayDuration = 900f;
    private float gameTime = 7f;
    private int day = 1;
    private bool isDayEnded = false;

    // 🔥 chỉ cho bấm Enter khi vừa hết ngày
    private bool canPressEnter = false;

    [Header("Lighting")]
    public Light sun;

    private Color lastSkyColor;

    void Start()
    {
        RenderSettings.skybox = new Material(RenderSettings.skybox);
    }

    void Update()
    {
        UpdateTime();
        UpdateUI();
        UpdateLighting();
        HandleEndDayInput();
    }

    void UpdateTime()
    {
        if (isDayEnded) return;

        float timeSpeed = (21f - 7f) / realDayDuration;
        gameTime += Time.deltaTime * timeSpeed;

        if (gameTime >= 21f)
        {
            gameTime = 21f;
            isDayEnded = true;

            // 🔥 chỉ bật Enter khi vừa tới 21h
            canPressEnter = true;
        }
    }

    void HandleEndDayInput()
    {
        if (isDayEnded && canPressEnter && Input.GetKeyDown(KeyCode.Return))
        {
            canPressEnter = false;
            NextDay();
        }
    }

    void NextDay()
    {
        day++;
        gameTime = 7f;
        isDayEnded = false;
        canPressEnter = false;

        // reset lighting update
        lastSkyColor = Color.clear;
    }

    void UpdateUI()
    {
        int hour = Mathf.FloorToInt(gameTime);
        int minute = Mathf.FloorToInt((gameTime - hour) * 60);

        timeText.text = $"Day {day} - {hour:00}:{minute:00}";

        if (isDayEnded)
        {
            timeText.text += "\n<color=yellow>Press ENTER to end day</color>";
        }
    }

    void UpdateLighting()
    {
        Color skyColor;

        Color dayColor = new Color(0.5f, 0.7f, 1f);
        Color sunsetColor = new Color(1f, 0.45f, 0.25f);
        Color nightColor = new Color(0.02f, 0.02f, 0.08f);

        float tDay = Mathf.InverseLerp(7f, 21f, gameTime);

        // 🌤 7h → 16h
        if (gameTime < 16f)
        {
            skyColor = dayColor;
        }
        // 🌇 16h → 18h
        else if (gameTime < 18f)
        {
            float t = Mathf.InverseLerp(16f, 18f, gameTime);
            skyColor = Color.Lerp(dayColor, sunsetColor, t);
        }
        // 🌙 18h → 21h
        else
        {
            float t = Mathf.InverseLerp(18f, 21f, gameTime);
            skyColor = Color.Lerp(sunsetColor, nightColor, t);
        }

        // 🌤 Skybox
        RenderSettings.skybox.SetColor("_SkyTint", skyColor);
        RenderSettings.skybox.SetFloat("_AtmosphereThickness", 0.6f);
        RenderSettings.skybox.SetFloat("_Exposure", gameTime < 16f ? 1.2f : 1.2f - tDay * 0.7f);

        // 🌫 Ambient
        Color ambientDay = new Color(0.8f, 0.85f, 0.9f);
        RenderSettings.ambientLight = Color.Lerp(ambientDay, nightColor, tDay);

        // 🌞 Sun
        if (sun != null)
        {
            float sunAngle = Mathf.Lerp(10f, 170f, tDay);
            sun.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0);

            if (gameTime < 16f)
            {
                sun.color = Color.white;
                sun.intensity = 1.2f;
            }
            else if (gameTime < 18f)
            {
                float t = Mathf.InverseLerp(16f, 18f, gameTime);
                sun.color = Color.Lerp(Color.white, sunsetColor, t);
                sun.intensity = Mathf.Lerp(1.2f, 0.6f, t);
            }
            else
            {
                float t = Mathf.InverseLerp(18f, 21f, gameTime);
                sun.color = sunsetColor;
                sun.intensity = Mathf.Lerp(0.6f, 0.2f, t);
            }
        }

        // ⚡ tối ưu
        if (Vector4.Distance(skyColor, lastSkyColor) > 0.01f)
        {
            DynamicGI.UpdateEnvironment();
            lastSkyColor = skyColor;
        }
    }
}