using UnityEngine;

public class IntersectionManager : MonoBehaviour
{
    public TrafficLight horizontalLight;
    public TrafficLight verticalLight;

    public float greenTime = 10f;
    public float yellowTime = 2f;

    void Start()
    {
        StartCoroutine(ControlTraffic());
    }

    System.Collections.IEnumerator ControlTraffic()
    {
        while (true)
        {
            // ngang xanh
            horizontalLight.SetState(TrafficLight.LightState.Green);
            verticalLight.SetState(TrafficLight.LightState.Red);
            yield return new WaitForSeconds(greenTime);

            horizontalLight.SetState(TrafficLight.LightState.Yellow);
            yield return new WaitForSeconds(yellowTime);

            // dọc xanh
            horizontalLight.SetState(TrafficLight.LightState.Red);
            verticalLight.SetState(TrafficLight.LightState.Green);
            yield return new WaitForSeconds(greenTime);

            verticalLight.SetState(TrafficLight.LightState.Yellow);
            yield return new WaitForSeconds(yellowTime);
        }
    }
}