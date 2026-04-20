using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    // Player
    public float playerX, playerY, playerZ;
    public float rotY, rotX;

    public int money;

    public float volume;
    public float sensitivity;

    // 🕒 TIME
    public int day;
    public float timeOfDay;

    // 🍔 FOOD
    public List<FoodSaveData> foods;

    // 📦 BOX
    public List<BoxData> boxes;
}