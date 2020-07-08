using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hero : CharacterBase
{

    private Animator animator;
    private PlayerMovement pm;
    private AttackComponent ac;
    private void Start()
    {
        animator = this.GetComponent<Animator>();
        pm = this.GetComponent<PlayerMovement>();
        ac = this.GetComponent<AttackComponent>();

    }
    public override void Die()
    {
        base.Die();
        animator.SetBool("isDead", true);
        pm.enabled = false;
        ac.enabled = false;
        // Play Hero death Animation
    }
}
