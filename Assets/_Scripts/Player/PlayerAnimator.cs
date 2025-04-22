using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    // Setters
    [SerializeField] private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Animation parameters for state switching
        animator.SetFloat("yVelocity", PlayerMovement.Instance.GetVelocity().y);
        animator.SetBool("isGrounded", PlayerMovement.Instance.IsGrounded());
        animator.SetFloat("Speed", Mathf.Abs(PlayerInputs.Instance.GetHorizontal()));
        animator.SetBool("isDashing", PlayerMovement.Instance.GetIsDashing());
        animator.SetBool("isUsingGNL", PlayerAttack.Instance.IsUsingGNL());
    }
}
