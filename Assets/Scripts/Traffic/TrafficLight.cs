using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public enum LightState
    {
        Red,
        Yellow,
        Green
    }

    public LightState currentState;

    [Header("Renderers")]
    public Renderer redLight;
    public Renderer yellowLight;
    public Renderer greenLight;

    [Header("Materials")]
    public Material redOn;
    public Material yellowOn;
    public Material greenOn;
    public Material lightOff;

    void Start()
    {
        UpdateLights();
    }

    public void SetState(LightState newState)
    {
        currentState = newState;
        UpdateLights();
    }

    void UpdateLights()
    {
        // 🔴 RED
        redLight.material = (currentState == LightState.Red) ? redOn : lightOff;

        // 🟡 YELLOW
        yellowLight.material = (currentState == LightState.Yellow) ? yellowOn : lightOff;

        // 🟢 GREEN
        greenLight.material = (currentState == LightState.Green) ? greenOn : lightOff;
    }
}