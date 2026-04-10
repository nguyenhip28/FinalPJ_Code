using UnityEngine;

public class ArrowGuide : MonoBehaviour
{
    public Transform target;   // nhà hàng
    public Transform player;   // player

    void Update()
    {
        if (target == null || player == null) return;

        Vector3 direction = target.position - player.position;
        direction.y = 0; // không cho ngửa lên trời

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);
        }
    }
}