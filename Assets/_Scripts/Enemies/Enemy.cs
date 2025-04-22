using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    // Setters
    public int knockback = 0;
    protected float speed;
    private Vector3 position;
    private bool isFacingRight = true;
    private bool isAttacked;
    protected bool isKnockedBack;
    protected Rigidbody2D rb;

    // Getters
    public Vector3 GetPosition(){
        return position;
    }

    // Functions
    protected void FollowPlayer(){
        if(PlayerMovement.Instance.GetPosition().x <= position.x){
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
        }
        else if (PlayerMovement.Instance.GetPosition().x >= position.x){
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
        }
    }
    protected void Flip(){
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1f; 
        transform.localScale = scale;
    }
    public IEnumerator TakeDamage(int attackDamage, int knockbackForce){
        knockback += attackDamage;
        isAttacked = true;
        
        // Stop movement
        Vector2 originalVelocity = rb.linearVelocity; 
        rb.linearVelocity = Vector2.zero; 
        rb.gravityScale = 0f; 

        yield return new WaitForSeconds(2f);

        rb.gravityScale = 1f;
        isAttacked = false;
        isKnockedBack = true;
        // Apply knockback
        int direction = (PlayerMovement.Instance.GetPosition().x < position.x) ? 1 : -1;
        rb.linearVelocity = new Vector2(knockback * knockbackForce * direction, rb.linearVelocity.y);
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Knockback wait
        if (isKnockedBack)
        {
            if (Mathf.Abs(rb.linearVelocity.x) < 0.1f)
            {
                isKnockedBack = false;
            }
            else
            {
                return; 
            }
        }
        if(isAttacked){
            return;
        }
        FollowPlayer();
        // Update Enemy position
        position = transform.position;
        // Raycast for auto jump
        Vector2 rayDirection = (isFacingRight) ? Vector2.right : Vector2.left;
        RaycastHit2D groundHit = Physics2D.Raycast(position, rayDirection, 3f, LayerManager.Instance.GetGroundLayer());
        if(groundHit.collider != null){
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 6f);
        }
        if (isFacingRight && rb.linearVelocity.x < 0f || !isFacingRight && rb.linearVelocity.x > 0f){
            Flip();
        }
    }

    // Physics Updates
    private void FixedUpdate(){
        if (isAttacked){
            rb.linearVelocity = Vector2.zero;
            return;
        }
        if (isKnockedBack)
        {
            return;
        }
    }
    
}
