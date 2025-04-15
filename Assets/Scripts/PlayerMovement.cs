using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{   
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
    // Dash variables
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    private bool hasDashedInAir = false; // added flag to track if dashed in air

    // Update is called once per frame
    void Update()
    {
        if (isDashing){
            return;
        }

        // Player movement inputs
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        float fallSpeed = (vertical < 0) ? -speed : rb.linearVelocity.y;
        rb.linearVelocity = new Vector2(horizontal * speed, fallSpeed);
        // dashing logic
        if(Input.GetKeyDown(KeyCode.L) && canDash){
            StartCoroutine(Dash());
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
        animator.SetBool("isDashing", isDashing);
    }
    private void FixedUpdate(){
        if(isDashing){
            return;
        }
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
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
    private IEnumerator Dash(){
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        hasDashedInAir = !isGrounded(); // record if it was an air dash
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        rb.gravityScale = originalGravity;

        if (hasDashedInAir){
            // Wait until grounded, then apply cooldown
            yield return new WaitUntil(() => isGrounded());
            yield return new WaitForSeconds(dashingCooldown);
        } else {
            // Ground dash: apply cooldown immediately
            yield return new WaitForSeconds(dashingCooldown);
        }

        canDash = true;
    }
}
