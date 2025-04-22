using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{   
    public Vector3 position;
    private float horizontal; 
    private float vertical; 
    public float speed = 10f; 
    private bool isFacingRight = true;
    private float jumpForce = 12f;
    [SerializeField] private Rigidbody2D rb; 
    [SerializeField] private Transform groundCheck;
    [SerializeField] public LayerMask groundLayer; 
    [SerializeField] private Animator animator;
    [SerializeField] public LayerMask enemyLayer; 
    [SerializeField] private Goblin goblin;
    [SerializeField] private AttackCollider attackCollider;
    // Dash variables
    private bool canUseAbility = true;
    private bool isDashing;
    private float dashingPower = 24f;
    public float abilityTime = 2f;
    private float dashingCooldown = 1f;
    private bool hasDashedInAir = false;
    private bool isUsingGNL;

    // Update is called once per frame
    void Update()
    {
        if (isDashing || isUsingGNL){
            return;
        }
        // Attack
        if(Input.GetKeyDown(KeyCode.J)  && horizontal == 0f && vertical >= 0f){
            StartCoroutine(GroundNeutralLight());
        }
        // Player movement inputs
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        float fallSpeed = (vertical < 0) ? -speed : rb.linearVelocity.y;
        rb.linearVelocity = new Vector2(horizontal * speed, fallSpeed);
        // dashing
        if(Input.GetKeyDown(KeyCode.L) && canUseAbility){
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
        animator.SetBool("isUsingGNL", isUsingGNL);
        // Update Player Position
        position = transform.position;
    }
    private void FixedUpdate(){
        if (isDashing || isUsingGNL){
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
        canUseAbility = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        hasDashedInAir = !isGrounded(); // record if it was an air dash
        yield return new WaitForSeconds(abilityTime);
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

        canUseAbility = true;
    }
    private IEnumerator GroundNeutralLight(){
        canUseAbility = false;
        isUsingGNL = true;
        // Stop movement
        Vector2 originalVelocity = rb.linearVelocity; 
        rb.linearVelocity = Vector2.zero; 
        rb.gravityScale = 0f; 
        // So that enemy gets damaged if it is in range and player is facing in its direction
        if((isFacingRight && position.x <= goblin.position.x) || (!isFacingRight && position.x >= goblin.position.x)){
            foreach (Goblin goblin in attackCollider.enemiesInRange)
            {
                StartCoroutine(goblin.TakeDamage(1));
            } 
        }
        

        yield return new WaitForSeconds(abilityTime);

        rb.gravityScale = 1f; 
        canUseAbility = true;
        isUsingGNL = false;
    }
}
