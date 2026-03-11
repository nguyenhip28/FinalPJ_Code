using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject darkOverlay;

    void Start()
    {
        settingsPanel.SetActive(false);
    }

    public void NewGame()
    {
        SceneManager.LoadScene("FinalProject");
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