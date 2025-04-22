using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{   
    // Singleton
    public static PlayerMovement Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Setters
    private Vector3 position;
    private float speed = 10f; 
    private bool isFacingRight = true;
    private float jumpForce = 12f;
    [SerializeField] private Rigidbody2D rb; 
    [SerializeField] private Transform groundCheck;
    private bool canDash = true;
    private bool isDashing;
    private float dashingCooldown = 1f;
    private bool hasDashedInAir = false;
    private bool isUsingGNL;
    // Getters
    public Vector3 GetPosition(){
        return position;
    }
    public bool GetIsFacingRight(){
        return isFacingRight;
    }
    public Vector2 GetVelocity(){
        return rb.linearVelocity;
    }
    public bool GetIsDashing(){
        return isDashing;
    }
    // Functions
    void Flip(){
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale; // tells you x,y,z scale of the object and saves 
        scale.x *= -1f; // flip x
        transform.localScale = scale; // apply it
    }
    // Checks if the player is touching the ground using an object under its feet
    public bool IsGrounded(){
        return Physics2D.OverlapCircle(groundCheck.position,0.2f,LayerManager.Instance.GetGroundLayer());
    }
    void Jump(){
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
    private IEnumerator Dash(){
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(transform.localScale.x * 12f, 0f);
        hasDashedInAir = !IsGrounded(); // record if it was an air dash
        yield return new WaitForSeconds(2f);
        isDashing = false;
        rb.gravityScale = originalGravity;

        if (hasDashedInAir){
            // Wait until grounded, then apply cooldown
            yield return new WaitUntil(() => IsGrounded());
            yield return new WaitForSeconds(dashingCooldown);
        } else {
            // Ground dash: apply cooldown immediately
            yield return new WaitForSeconds(dashingCooldown);
        }

        canDash = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (isDashing || PlayerAttack.Instance.GetIsAttacking()){
            return;
        }
        
        float fallSpeed = (PlayerInputs.Instance.GetVertical() < 0) ? -speed : rb.linearVelocity.y;
        rb.linearVelocity = new Vector2(PlayerInputs.Instance.GetHorizontal() * speed, fallSpeed);
        // dashing
        if(Input.GetKeyDown(KeyCode.L) && canDash){
            StartCoroutine(Dash());
        }
        

        // Check if sprite flip is necesarry accordint to movement
        if (isFacingRight && PlayerInputs.Instance.GetHorizontal() < 0f || !isFacingRight && PlayerInputs.Instance.GetHorizontal() > 0f){
            Flip();
        }
        // Jump check
        if (Input.GetButtonDown("Jump") && IsGrounded()){
            Jump();
        }
        // Update Player Position
        position = transform.position;
    }
    private void FixedUpdate(){
        if (isDashing || PlayerAttack.Instance.GetIsAttacking()){
            return;
        }
        rb.linearVelocity = new Vector2(PlayerInputs.Instance.GetHorizontal() * speed, rb.linearVelocity.y);
    }
    
    
}
