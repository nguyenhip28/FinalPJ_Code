using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public float playerX, playerY, playerZ;
    public float rotY, rotX;

    public int money;

    public float volume;
    public float sensitivity;

    public int day;
    public float timeOfDay;

    public List<FoodSaveData> foods;

    public List<BoxData> boxes;
}