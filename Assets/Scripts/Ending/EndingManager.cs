using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject popupPanel;

    void Start()
    {
        // 👉 đăng ký sự kiện khi video kết thúc
        videoPlayer.loopPointReached += OnVideoEnd;

        popupPanel.SetActive(false);
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // 👉 hiện popup
        popupPanel.SetActive(true);
    }

    // 👉 nút YES
    public void OnYes()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // 👉 nút NO
    public void OnNo()
    {
        SceneManager.LoadScene("FinalProject");
    }
}