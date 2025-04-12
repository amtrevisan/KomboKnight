using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal; 
    private float vertical; 
    public float speed = 10f; 
    private bool isFacingRight = true;
    public float jumpForce = 12f;

    [SerializeField] private Rigidbody2D rb; // Reference to  the rigidBody attatched to the player
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer; 

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        float fallSpeed = (vertical < 0) ? -speed : rb.linearVelocity.y;
        rb.linearVelocity = new Vector2(horizontal * speed, fallSpeed);
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f){
            Flip();
        }
        if (Input.GetButtonDown("Jump") && isGrounded()){
            Jump();
        }
    }
    void Flip(){
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale; // tells you x,y,z scale of the object and saves 
        scale.x *= -1f; // flip x
        transform.localScale = scale; // apply it
    }
    private bool isGrounded(){
        return Physics2D.OverlapCircle(groundCheck.position,0.2f,groundLayer);
    }
    void Jump(){
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
}
