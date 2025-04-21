using UnityEngine;
public class Goblin : MonoBehaviour
{
    private int health = 100;
    private int damage = 10;
    private float speed = 5f;
    private Vector3 position;
    private bool isFacingRight = true;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        followPlayer();
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
    void takeDamage(){
        health -= player.damage;
    }
    void followPlayer(){
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
