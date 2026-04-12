[System.Serializable]
public class FeeData
{
    public string feeName;
    public int amount;
    public bool isPaid;

    public FeeData(string name, int amt)
    {
        feeName = name;
        amount = amt;
        isPaid = false;
    }
}