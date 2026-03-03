using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void UpdateMovement(float speed)
    {
        animator.SetFloat("Speed", speed, 0.15f, Time.deltaTime);
    }

    public void SetGrounded(bool grounded)
    {
        animator.SetBool("IsGrounded", grounded);
    }

    public void Jump()
    {
        animator.SetTrigger("Jump");
    }
}
