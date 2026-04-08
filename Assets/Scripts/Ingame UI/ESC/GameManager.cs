using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Transform player;
    public MouseLook mouseLook;

    private CharacterController cc;
    private Rigidbody rb;

    void Awake()
    {
        cc = player.GetComponent<CharacterController>();
        rb = player.GetComponent<Rigidbody>();
    }

    void Start()
    {
        // 🔥 Nếu là New Game → KHÔNG load save
        if (PlayerPrefs.GetInt("NewGame", 0) == 1)
        {
            PlayerPrefs.SetInt("NewGame", 0);
            Debug.Log("New Game Started");
            return;
        }

        // 🔥 Continue thì mới load
        if (SaveSystem.HasSave())
        {
            StartCoroutine(LoadAfterFrame());
        }
    }

    IEnumerator LoadAfterFrame()
    {
        yield return null; // 🔥 tránh giật
        LoadGame();
    }

    public void SaveGame()
    {
        GameData data = new GameData();

        // 📍 Position
        data.playerX = player.position.x;
        data.playerY = player.position.y;
        data.playerZ = player.position.z;

        // 🎥 Rotation
        data.rotY = player.eulerAngles.y;
        data.rotX = mouseLook.GetXRotation(); // 🔥 FIX

        // 💰 Money
        data.money = PlayerMoney.Instance.money;

        // 🔊 Settings
        data.volume = AudioListener.volume;
        data.sensitivity = mouseLook.mouseSensitivity;

        SaveSystem.Save(data);

        Debug.Log("Saved!");

        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGame()
    {
        GameData data = SaveSystem.Load();

        if (data == null)
        {
            Debug.Log("No save file!");
            return;
        }

        // 🔥 Disable controller
        if (cc != null) cc.enabled = false;
        if (rb != null) rb.isKinematic = true;

        // 📍 Position
        player.position = new Vector3(data.playerX, data.playerY, data.playerZ);

        // 🎥 Rotation Y (player)
        player.rotation = Quaternion.Euler(0f, data.rotY, 0f);

        // 🎥 Rotation X (camera)
        mouseLook.SetRotation(data.rotX);

        // 🔥 Enable lại
        if (cc != null) cc.enabled = true;
        if (rb != null) rb.isKinematic = false;

        // 💰 Money
        if (PlayerMoney.Instance != null)
            PlayerMoney.Instance.SetMoney(data.money);

        // 🔊 Settings
        AudioListener.volume = data.volume;
        mouseLook.UpdateSensitivity(data.sensitivity);

        Debug.Log("Game Loaded!");
    }
}