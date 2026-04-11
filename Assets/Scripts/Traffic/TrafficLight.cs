using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public enum LightState { Green, Yellow, Red }
    public LightState currentState;

    [Header("Renderers")]
    public Renderer redLight;
    public Renderer yellowLight;
    public Renderer greenLight;

    [Header("Materials")]
    public Material onMat;
    public Material offMat;

    public void SetState(LightState state)
    {
        currentState = state;
        UpdateVisual();
    }

    void UpdateVisual()
    {
        redLight.material = offMat;
        yellowLight.material = offMat;
        greenLight.material = offMat;

        if (currentState == LightState.Red)
            redLight.material = onMat;

        else if (currentState == LightState.Yellow)
            yellowLight.material = onMat;

        else if (currentState == LightState.Green)
            greenLight.material = onMat;
    }
}