using UnityEngine;
public class Goblin : MonoBehaviour
{
    private int health = 100;
    private int damage = 10;
    private float speed = 5f;
    private Vector3 position;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Goblin" +  position);
        Debug.Log("Player" + player.position);
        followPlayer();
        // Update Goblin position
        position = transform.position;
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
}
