using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    private InputAction moveAction;

    void Awake()
    {
        var inputActions = new InputSystem_Actions();
        moveAction = inputActions.Player.Move;
    }

    void OnEnable()
    {
        moveAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
    }

    void Update()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();

        // 👉 hướng theo thân Player
        Vector3 move =
            transform.forward * input.y +
            transform.right * input.x;

        transform.position += move * speed * Time.deltaTime;
    }
}
