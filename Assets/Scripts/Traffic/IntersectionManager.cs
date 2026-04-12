using UnityEngine;
using System.Collections;

public class IntersectionManager : MonoBehaviour
{
    public TrafficLight horizontalLight;
    public TrafficLight verticalLight;

    public float greenTime = 8f;
    public float yellowTime = 2f;

    void Start()
    {
        StartCoroutine(ControlTraffic());
    }

    IEnumerator ControlTraffic()
    {
        while (true)
        {
            // ngang xanh
            horizontalLight.SetState(TrafficLight.LightState.Green);
            verticalLight.SetState(TrafficLight.LightState.Red);
            yield return new WaitForSeconds(greenTime);

            // ngang vàng
            horizontalLight.SetState(TrafficLight.LightState.Yellow);
            yield return new WaitForSeconds(yellowTime);

            // dọc xanh
            horizontalLight.SetState(TrafficLight.LightState.Red);
            verticalLight.SetState(TrafficLight.LightState.Green);
            yield return new WaitForSeconds(greenTime);

            // dọc vàng
            verticalLight.SetState(TrafficLight.LightState.Yellow);
            yield return new WaitForSeconds(yellowTime);
        }
    }
}