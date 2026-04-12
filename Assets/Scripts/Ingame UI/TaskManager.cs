using UnityEngine;
using TMPro;

public class TaskManager : MonoBehaviour
{
    public TextMeshProUGUI findRestaurantText;
    public TextMeshProUGUI orderText;
    public TextMeshProUGUI tacoText;
    public GameObject taskPanel;

    private bool foundRestaurant = false;
    private bool ordered = false;
    private int tacoProgress = 0;

    public void CompleteFindRestaurant()
    {
        foundRestaurant = true;
        findRestaurantText.text = "[X] Find your restaurant";
        CheckAllTasksComplete();
    }

    public void CompleteOrder()
    {
        ordered = true;
        orderText.text = "[X] Try to order on computer";
        CheckAllTasksComplete();
    }

    public void AddTacoStep()
    {
        tacoProgress++;
        tacoText.text = $"[ ] Try make the first taco ({tacoProgress}/4)";

        if (tacoProgress >= 4)
        {
            tacoText.text = "[X] Try make the first taco";
            CheckAllTasksComplete();
        }
    }
    void CheckAllTasksComplete()
    {
        if (foundRestaurant && ordered && tacoProgress >= 4)
        {
            taskPanel.SetActive(false);
        }
    }
}