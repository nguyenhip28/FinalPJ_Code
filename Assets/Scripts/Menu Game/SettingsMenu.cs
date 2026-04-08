using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider sensitivitySlider;

    public TMP_Text volumeValueText;
    public TMP_Text sensitivityValueText;

    public MouseLook mouseLook;

    void Start()
    {
        float volume = PlayerPrefs.GetFloat("volume", 1f);
        float sensitivity = PlayerPrefs.GetFloat("sensitivity", 100f);

        volumeSlider.value = volume;
        sensitivitySlider.value = sensitivity;

        AudioListener.volume = volume;

        UpdateVolumeText(volume);
        UpdateSensitivityText(sensitivity);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("volume", volume);

        UpdateVolumeText(volume);
    }

    public void SetSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("sensitivity", sensitivity);

        if (mouseLook != null)
            mouseLook.UpdateSensitivity(sensitivity);

        UpdateSensitivityText(sensitivity);
    }

    void UpdateVolumeText(float volume)
    {
        volumeValueText.text = Mathf.RoundToInt(volume * 100) + "%";
    }

    void UpdateSensitivityText(float sensitivity)
    {
        sensitivityValueText.text = Mathf.RoundToInt(sensitivity).ToString();
    }
}