using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Goblin : Enemy
{
    [SerializeField] private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = 7f;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        animator.SetBool("isRunning", isMoving);
        animator.SetBool("isHit", isAttacked);
        animator.SetBool("isKnockedBack", isKnockedBack);
    }
    private void FixedUpdate(){
        base.FixedUpdate();
    }
}
