using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/save.json";

    public static void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);

        Debug.Log("Saved to: " + path);
    }

    public static GameData Load()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<GameData>(json);
        }

        return null;
    }

    public static bool HasSave()
    {
        return File.Exists(path);
    }

    public static void DeleteSave()
    {
        string path = Application.persistentDataPath + "/save.json";

        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
            Debug.Log("Save deleted!");
        }
    }
}