[System.Serializable]
public class TacoData
{
    public int meat;
    public bool lettuce;
    public bool tomato;

    public bool Compare(TacoData other)
    {
        return meat == other.meat &&
               lettuce == other.lettuce &&
               tomato == other.tomato;
    }
}