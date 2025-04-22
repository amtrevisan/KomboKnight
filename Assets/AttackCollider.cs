using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackCollider : MonoBehaviour
{
    public List<Goblin> enemiesInRange = new List<Goblin>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Goblin goblin = other.GetComponent<Goblin>();
            if (goblin != null && !enemiesInRange.Contains(goblin))
            {
                enemiesInRange.Add(goblin);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Goblin goblin = other.GetComponent<Goblin>();
            if (goblin != null && enemiesInRange.Contains(goblin))
            {
                enemiesInRange.Remove(goblin);
            }
        }
    }
}
