using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    // Singleton
    public static PlayerInputs Instance { get; private set; }
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
    private float horizontal; 
    private float vertical; 
    // Getters
    public float GetHorizontal(){
        return horizontal;
    }
    public float GetVertical(){
        return vertical;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Player movement inputs
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }
}
