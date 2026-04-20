using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public Transform player;
    public static bool isNewGame = false;
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
        int newGameFlag = PlayerPrefs.GetInt("NewGame", 0);

        Debug.Log("NewGame flag = " + newGameFlag);

        if (newGameFlag == 1)
        {
            Debug.Log("NEW GAME → SKIP LOAD");

            PlayerPrefs.SetInt("NewGame", 0);
            PlayerPrefs.Save();
            return;
        }

        if (SaveSystem.HasSave())
        {
            Debug.Log("CONTINUE → LOAD");
            StartCoroutine(LoadAfterFrame());
        }
    }

    IEnumerator LoadAfterFrame()
    {
        yield return null;
        LoadGame();
    }

    // ================= SAVE =================

    public void SaveGame()
    {
        GameData data = new GameData();

        // 📍 PLAYER POSITION
        data.playerX = player.position.x;
        data.playerY = player.position.y;
        data.playerZ = player.position.z;

        // 🎥 ROTATION
        data.rotY = player.eulerAngles.y;
        data.rotX = mouseLook.GetXRotation();

        // 💰 MONEY
        data.money = PlayerMoney.Instance.money;

        // 🔊 SETTINGS
        data.volume = AudioListener.volume;
        data.sensitivity = mouseLook.mouseSensitivity;

        // ================= 🕒 TIME =================
        TimeManager tm = UnityEngine.Object.FindFirstObjectByType<TimeManager>();
        data.day = tm.GetDay();
        data.timeOfDay = tm.GetTime();

        // ================= 🍔 FOOD =================
        data.foods = new List<FoodSaveData>();

        FoodItem[] foods = UnityEngine.Object.FindObjectsByType<FoodItem>(FindObjectsSortMode.None);
        foreach (var food in foods)
        {
            data.foods.Add(food.GetData());
        }

        // ================= 📦 BOX =================
        data.boxes = new List<BoxData>();

        FoodBox[] boxes = UnityEngine.Object.FindObjectsByType<FoodBox>(FindObjectsSortMode.None);
        foreach (var box in boxes)
        {
            data.boxes.Add(box.GetData());
        }

        // ================= SAVE FILE =================
        SaveSystem.Save(data);

        Debug.Log("Saved!");

        SceneManager.LoadScene("MainMenu");
    }

    // ================= LOAD =================

    public void LoadGame()
    {
        if (PlayerPrefs.GetInt("NewGame", 0) == 1)
        {
            Debug.Log("BLOCK LOAD (NEW GAME)");
            return;
        }

        GameData data = SaveSystem.Load();

        if (data == null)
        {
            Debug.Log("No save file!");
            return;
        }

        // 🔥 Disable controller
        if (cc != null) cc.enabled = false;
        if (rb != null) rb.isKinematic = true;

        // 📍 POSITION
        player.position = new Vector3(data.playerX, data.playerY, data.playerZ);

        // 🎥 ROTATION
        player.rotation = Quaternion.Euler(0f, data.rotY, 0f);
        mouseLook.SetRotation(data.rotX);

        // 🔥 Enable lại
        if (cc != null) cc.enabled = true;
        if (rb != null) rb.isKinematic = false;

        // 💰 MONEY
        if (PlayerMoney.Instance != null)
            PlayerMoney.Instance.SetMoney(data.money);

        // 🔊 SETTINGS
        AudioListener.volume = data.volume;
        mouseLook.UpdateSensitivity(data.sensitivity);

        // ================= 🕒 TIME =================
        TimeManager tm = UnityEngine.Object.FindFirstObjectByType<TimeManager>();
        tm.LoadTime(data.day, data.timeOfDay);

        // ================= 🍔 FOOD =================

        // Xoá toàn bộ food cũ
        foreach (var f in UnityEngine.Object.FindObjectsByType<FoodItem>(FindObjectsSortMode.None))
        {
            Destroy(f.gameObject);
        }

        // Spawn lại
        foreach (var f in data.foods)
        {
            GameObject prefab = FoodDatabase.Instance.GetPrefab((FoodType)f.foodType);

            GameObject obj = Instantiate(prefab);
            FoodItem food = obj.GetComponent<FoodItem>();

            food.LoadFromData(f);
        }

        // ================= 📦 BOX =================

        FoodBox[] boxes = UnityEngine.Object.FindObjectsByType<FoodBox>(FindObjectsSortMode.None);

        foreach (var box in boxes)
        {
            foreach (var saved in data.boxes)
            {
                if (box.boxID == saved.boxID)
                {
                    box.LoadFromData(saved);
                    break;
                }
            }
        }

        Debug.Log("Game Loaded!");
    }
}