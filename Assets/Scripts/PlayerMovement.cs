using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal; 
    private float vertical; 
    public float speed = 10f; 
    private bool isFacingRight = true;
    public float jumpForce = 12f;
    [SerializeField] private Rigidbody2D rb; 
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer; 
    [SerializeField] private Animator animator;

    // Update is called once per frame
    void Update()
    {
        // Player movement inputs
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        // Fast Fall mechanic
        float fallSpeed = (vertical < 0) ? -speed : rb.linearVelocity.y;
        rb.linearVelocity = new Vector2(horizontal * speed, fallSpeed);
        // Check if sprite flip is necesarry accordint to movement
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f){
            Flip();
        }
        // Jump check
        if (Input.GetButtonDown("Jump") && isGrounded()){
            Jump();
        }
        // Animation parameters for state switching
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetBool("isGrounded", isGrounded());
    }
    void Flip(){
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale; // tells you x,y,z scale of the object and saves 
        scale.x *= -1f; // flip x
        transform.localScale = scale; // apply it
    }
    // Checks if the player is touching the ground using an object under its feet
    private bool isGrounded(){
        return Physics2D.OverlapCircle(groundCheck.position,0.2f,groundLayer);
    }
    void Jump(){
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
}
