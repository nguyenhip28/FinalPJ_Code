using UnityEngine;

public class RestaurantTrigger : MonoBehaviour
{
    public TaskManager taskManager;
    public GameObject arrow;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            taskManager.CompleteFindRestaurant();
            arrow.SetActive(false);
        }
    }
}