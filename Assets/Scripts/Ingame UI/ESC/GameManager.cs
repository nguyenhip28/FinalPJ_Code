using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

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
        int newGameFlag = PlayerPrefs.GetInt("NewGame", 0);

        if (newGameFlag == 1)
        {
            PlayerPrefs.SetInt("NewGame", 0);
            PlayerPrefs.Save();
            return;
        }

        if (SaveSystem.HasSave())
        {
            StartCoroutine(LoadAfterFrame());
        }
    }

    IEnumerator LoadAfterFrame()
    {
        yield return null;
        LoadGame();
    }


    public void SaveGame()
    {
        GameData data = new GameData();

        data.playerX = player.position.x;
        data.playerY = player.position.y;
        data.playerZ = player.position.z;

        data.rotY = player.eulerAngles.y;
        data.rotX = mouseLook.GetXRotation();

        data.money = PlayerMoney.Instance.money;

        data.volume = AudioListener.volume;

        data.sensitivity = mouseLook.mouseSensitivity;

        TimeManager tm = UnityEngine.Object.FindFirstObjectByType<TimeManager>();
        if (tm != null)
        {
            data.day = tm.GetDay();
            data.timeOfDay = tm.GetTime();
        }

        data.foods = new List<FoodSaveData>();
        foreach (var food in UnityEngine.Object.FindObjectsByType<FoodItem>(FindObjectsSortMode.None))
        {
            data.foods.Add(food.GetData());
        }


        data.boxes = new List<BoxData>();
        foreach (var box in UnityEngine.Object.FindObjectsByType<FoodBox>(FindObjectsSortMode.None))
        {
            data.boxes.Add(box.GetData());
        }


        SaveSystem.Save(data);

        Debug.Log("GAME SAVED");

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

        
        if (cc != null) cc.enabled = false;
        if (rb != null) rb.isKinematic = true;

        
        player.position = new Vector3(data.playerX, data.playerY, data.playerZ);
        player.rotation = Quaternion.Euler(0f, data.rotY, 0f);
        mouseLook.SetRotation(data.rotX);

        
        if (cc != null) cc.enabled = true;
        if (rb != null) rb.isKinematic = false;

        
        if (PlayerMoney.Instance != null)
            PlayerMoney.Instance.SetMoney(data.money);

        
        AudioListener.volume = data.volume;
        mouseLook.UpdateSensitivity(data.sensitivity);

        
        TimeManager tm = UnityEngine.Object.FindFirstObjectByType<TimeManager>();
        if (tm != null)
        {
            tm.LoadTime(data.day, data.timeOfDay);
        }

        
        foreach (var f in UnityEngine.Object.FindObjectsByType<FoodItem>(FindObjectsSortMode.None))
        {
            Destroy(f.gameObject);
        }

        
        foreach (var b in UnityEngine.Object.FindObjectsByType<FoodBox>(FindObjectsSortMode.None))
        {
            Destroy(b.gameObject);
        }

        
        foreach (var f in data.foods)
        {
            GameObject prefab = FoodDatabase.Instance.GetPrefab((FoodType)f.foodType);

            GameObject obj = Instantiate(prefab);
            obj.GetComponent<FoodItem>().LoadFromData(f);
        }

        
        foreach (var b in data.boxes)
        {
            GameObject prefab = BoxDatabase.Instance.GetPrefab((BoxType)b.boxType);

            GameObject obj = Instantiate(prefab);
            obj.GetComponent<FoodBox>().LoadFromData(b);
        }

        Debug.Log("GAME LOADED");
    }
}