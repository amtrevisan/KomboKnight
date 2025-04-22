using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform player;
    
    // Update is called once per frame
    void Update()
    {
        // Follow player logic
        transform.position = new Vector3(player.position.x, player.position.y, -10);
    }
}
