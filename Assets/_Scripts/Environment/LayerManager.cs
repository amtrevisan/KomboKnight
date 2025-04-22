using UnityEngine;

public class LayerManager : MonoBehaviour
{
    // Singleton
    public static LayerManager Instance { get; private set; }
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
    [SerializeField] private LayerMask groundLayer; 
    // Getters
    public LayerMask GetGroundLayer(){
        return groundLayer;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
