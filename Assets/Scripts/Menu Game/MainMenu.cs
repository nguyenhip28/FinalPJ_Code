using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject darkOverlay;

    public IntroManager introManager; 

    void Start()
    {
        settingsPanel.SetActive(false);
    }

    public void NewGame()
    {
        SaveSystem.DeleteSave();

        PlayerPrefs.SetInt("NewGame", 1);
        PlayerPrefs.Save();

        MusicManager music = Object.FindFirstObjectByType<MusicManager>();
        if (music != null)
        {
            music.StopMusic();
        }

        SceneManager.LoadScene("FinalProject");
    }

    public void ContinueGame()
    {
        if (SaveSystem.HasSave())
        {
            PlayerPrefs.SetInt("NewGame", 0); 
            SceneManager.LoadScene("FinalProject");
        }
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