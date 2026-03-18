using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource;

    public AudioClip menuMusic;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        PlayMenuMusic();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);

        if (scene.name == "MainMenu")
        {
            PlayMenuMusic();
        }
        else if (scene.name == "FinalProject")
        {
            // 🔥 KHÔNG LÀM GÌ CẢ
            // nhạc game sẽ do AudioSource trong scene xử lý
        }
    }

    // 🎵 MENU ONLY
    void PlayMenuMusic()
    {
        if (menuMusic == null) return;

        audioSource.Stop();
        audioSource.clip = menuMusic;
        audioSource.time = 0f;

        audioSource.volume = 0.5f;
        audioSource.loop = true;
        audioSource.Play();

        Debug.Log("Play MENU music");
    }

    // 🔥 dùng khi bấm NewGame
    public void StopMusic()
    {
        audioSource.Stop();
    }
}