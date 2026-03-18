using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject darkOverlay;

    public IntroManager introManager; // 🔥 THÊM DÒNG NÀY

    void Start()
    {
        settingsPanel.SetActive(false);
    }

    public void NewGame()
    {
        // 🔥 Tắt nhạc menu
        MusicManager music = FindObjectOfType<MusicManager>();
        if (music != null)
        {
            music.StopMusic();
        }

        // 🔥 Chạy intro
        introManager.PlayIntro();
    }

    public void ContinueGame()
    {
        Debug.Log("Continue Game");
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        darkOverlay.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        darkOverlay.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}