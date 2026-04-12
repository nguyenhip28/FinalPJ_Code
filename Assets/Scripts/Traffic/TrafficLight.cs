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
    public MeshRenderer redLight;
    public MeshRenderer yellowLight;
    public MeshRenderer greenLight;

    [Header("Materials")]
    public Material redOn;
    public Material yellowOn;
    public Material greenOn;
    public Material lightOff;

    public void SetState(LightState state)
    {
        currentState = state;
        UpdateVisual();
    }

    void UpdateVisual()
    {
        redLight.material = (currentState == LightState.Red) ? redOn : lightOff;
        yellowLight.material = (currentState == LightState.Yellow) ? yellowOn : lightOff;
        greenLight.material = (currentState == LightState.Green) ? greenOn : lightOff;
    }
}