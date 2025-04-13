using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    public float lastDashTime;
    public Vector3 playerPos;
    private float horizontal; 
    private float vertical; 
    public float speed = 10f; 
    private bool isFacingRight = true;
    public float jumpForce = 12f;
    [SerializeField] private Rigidbody2D rb; 
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer; 
    [SerializeField] private Animator animator;

    private bool isDashing = false;
    private float dashCooldown = 1f;
    private float dashDuration = 0.2f;

    // Update is called once per frame
    void Update()
    {
        // Player movement inputs
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        // Dashing logic
        if (isDashing && Time.time >= lastDashTime + dashDuration) {
            isDashing = false;
        }

        if (Input.GetKeyDown(KeyCode.L) && horizontal != 0 && isGrounded() && Time.time >= lastDashTime + dashCooldown) {
            isDashing = true;
            lastDashTime = Time.time;
            rb.linearVelocity = new Vector2(horizontal * 25f, rb.linearVelocity.y);
            Debug.Log("Dash Triggered");
        }
        // Default movement
        if (!isDashing) {
            float fallSpeed = (vertical < 0) ? -speed : rb.linearVelocity.y;
            rb.linearVelocity = new Vector2(horizontal * speed, fallSpeed);
        }

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
        animator.SetFloat("Speed", Mathf.Abs(horizontal));
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
