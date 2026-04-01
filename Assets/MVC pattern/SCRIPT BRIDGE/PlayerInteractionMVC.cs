using UnityEngine;

public class PlayerInteractionMVC : MonoBehaviour
{
    public float interactDistance = 3f;
    public Camera playerCamera;
    public Transform holdPoint;
    public Transform boxPoint;

    private PlayerInteractionModel model;
    private PlayerInteractionController controller;

    private PlayerInteractionView view;

    void Awake()
    {
        model = new PlayerInteractionModel();
        view = GetComponent<PlayerInteractionView>();

        controller = new PlayerInteractionController(
            model,
            view,
            playerCamera,
            interactDistance,
            holdPoint,
            boxPoint
        );
    }

    void Update()
    {
        controller.Update();
    }

    void LateUpdate()
    {
        controller.LateUpdate();
    }
}