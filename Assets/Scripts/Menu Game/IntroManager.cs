using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public GameObject introPanel;
    public VideoPlayer videoPlayer;
    public string gameSceneName = "FinalProject";

    private bool introPlaying = false;

    public void PlayIntro()
    {
        introPanel.SetActive(true);
        videoPlayer.Play();

        introPlaying = true;

        videoPlayer.loopPointReached += EndVideo;
    }

    void EndVideo(VideoPlayer vp)
    {
        SceneManager.LoadScene(gameSceneName);
    }

    void Update()
    {
        if (introPlaying && Input.anyKeyDown)
        {
            SceneManager.LoadScene(gameSceneName);
        }
    }
}