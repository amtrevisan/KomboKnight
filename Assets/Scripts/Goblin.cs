using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Goblin : MonoBehaviour
{
    private int knockback = 0;
    private float speed = 5f;
    public Vector3 position;
    private bool isFacingRight = true;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Rigidbody2D rb;
    private bool isAttacked;
    private bool isKnockedBack;

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
        
        //Debug
        FollowPlayer();
        // Update Goblin position
        position = transform.position;
        // Raycast for auto jump
        Vector2 rayDirection = (isFacingRight) ? Vector2.right : Vector2.left;
        RaycastHit2D groundHit = Physics2D.Raycast(position, rayDirection, 3f, player.groundLayer);
        if(groundHit.collider != null){
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 6f);
        }
        if (isFacingRight && rb.linearVelocity.x < 0f || !isFacingRight && rb.linearVelocity.x > 0f){
            Flip();
        }
    }
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
    public IEnumerator TakeDamage(int attackDamage){
        knockback += attackDamage;
        isAttacked = true;
        
        Debug.Log(knockback);
        // Stop movement
        Vector2 originalVelocity = rb.linearVelocity; 
        rb.linearVelocity = Vector2.zero; 
        rb.gravityScale = 0f; 

        yield return new WaitForSeconds(player.abilityTime);

        rb.gravityScale = 1f;
        isAttacked = false;
        isKnockedBack = true;
        // Apply knockback
        int direction = (player.position.x < position.x) ? 1 : -1;
        rb.linearVelocity = new Vector2(knockback * direction, rb.linearVelocity.y);
    }
    void FollowPlayer(){
        if(player.position.x <= position.x){
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
        }
        else if (player.position.x >= position.x){
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
        }
    }
    void Flip(){
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1f; 
        transform.localScale = scale;
    }
}
