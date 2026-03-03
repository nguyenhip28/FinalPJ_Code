using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float rotationSpeed = 6f;

    [Header("Jump & Gravity")]
    public float jumpHeight = 1.8f;
    public float gravity = -20f;

    private CharacterController controller;
    private Animator animator;
    private Transform cam;

    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        cam = Camera.main.transform;
    }

    void Update()
    {
        GroundCheck();
        Move();
        Jump();
        ApplyGravity();
    }

    // ===================== GROUND =====================
    void GroundCheck()
    {
        isGrounded = controller.isGrounded;
        animator.SetBool("IsGrounded", isGrounded);

        if (isGrounded && velocity.y < 0f)
            velocity.y = -2f;
    }

    // ===================== MOVE (TPS + STRAFE) =====================
    void Move()
    {
        float h = Input.GetAxis("Horizontal"); // A / D
        float v = Input.GetAxis("Vertical");   // W / S

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float speed = isRunning ? runSpeed : walkSpeed;

        // Camera-relative directions (không xoay theo A/D)
        Vector3 camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 camRight = cam.right;

        Vector3 moveDir = camForward * v + camRight * h;

        if (moveDir.magnitude > 0.1f)
        {
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

            // Chỉ xoay nhân vật theo hướng camera
            Quaternion targetRot = Quaternion.LookRotation(camForward);
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime
            );
        }

        // ===== ANIMATION (SMOOTH WALK ↔ RUN) =====
        bool hasInput = Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f;

        float targetAnimSpeed = 0f;
        if (hasInput)
            targetAnimSpeed = isRunning ? 2f : 1f;

        float currentAnimSpeed = animator.GetFloat("Speed");
        float smoothAnimSpeed = Mathf.Lerp(
            currentAnimSpeed,
            targetAnimSpeed,
            Time.deltaTime * 8f // chỉnh 6–10 nếu muốn
        );

        animator.SetFloat("Speed", smoothAnimSpeed);
    }

    // ===================== JUMP =====================
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("Jump");
        }
    }

    // ===================== GRAVITY =====================
    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
