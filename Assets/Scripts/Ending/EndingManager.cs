using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject popupPanel;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;

        popupPanel.SetActive(false);
    }

    void OnVideoEnd(VideoPlayer vp)
    {

        popupPanel.SetActive(true);
    }

    public void OnYes()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnNo()
    {
        SceneManager.LoadScene("FinalProject");
    }
}