using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour
{
    // Singleton
    public static PlayerAttack Instance { get; private set; }
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
    public int knockback;
    private bool canAttack;
    private bool isAttacking = false;
    private bool usingGNL;
    [SerializeField] private AttackCollider attackCollider;
    // Getters
    public bool GetIsAttacking(){
        return isAttacking;
    }
    public bool IsUsingGNL(){
        return usingGNL;
    }
    // helpers
    private bool IsEnemyInFront(Enemy enemy)
    {
        if (PlayerMovement.Instance.GetIsFacingRight()){
            return PlayerMovement.Instance.GetPosition().x < enemy.GetPosition().x;
        }     
        else{
            return PlayerMovement.Instance.GetPosition().x > enemy.GetPosition().x;
        }
    }
    // Attacks:
    private IEnumerator GroundNeutralLight(){
        canAttack = false;
        isAttacking = true;
        usingGNL = true;
        // So that enemy gets damaged if it is in range and player is facing in its direction
        foreach (Enemy enemy in attackCollider.enemiesInRange){
            Debug.Log(IsEnemyInFront(enemy));
            if (IsEnemyInFront(enemy)){
                StartCoroutine(enemy.TakeDamage(1, 1));
            }
        }
        
        yield return new WaitForSeconds(0.2f);

        
        canAttack = true;
        isAttacking = false;
        usingGNL = false;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Attacks
        if(Input.GetKeyDown(KeyCode.J)  && PlayerInputs.Instance.GetHorizontal() == 0f && PlayerInputs.Instance.GetVertical() >= 0f){
            StartCoroutine(GroundNeutralLight());
        }
    }
}
