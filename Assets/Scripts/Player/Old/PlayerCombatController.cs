using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField]
    private bool combatEnabled;
    [SerializeField]
    private float inputTimer, attack1Radius, attack1Damage;
    [SerializeField]
    private float stunDamageAmount = 1;
    [SerializeField]
    private Transform attack1HitBoxPos;
    [SerializeField]
    private LayerMask whatIsDamageable;
    private bool gotInput, isAttacking, isFirstAttack;

    private float lastInputTime = Mathf.NegativeInfinity;
    private AttackDetails attackDetails;

    private Animator anim;
    private PlayerController playerController;
    private PlayerStats playerStats;
    private void Start()
    {
        anim = GetComponent<Animator>();

        playerController = GetComponent<PlayerController>();
        playerStats = GetComponent<PlayerStats>();
        anim.SetBool("canAttack", combatEnabled);

    }
    private void Update()
    {
        CheckCombatInput();
        CheckAttacks();
    }

    private void CheckAttacks()
    {
        if (gotInput)
        {
            //perform attack1
            if (!isAttacking)
            {

                gotInput = false;
                isAttacking = true;
                isFirstAttack = !isFirstAttack;
                anim.SetBool("attack1", true);
                anim.SetBool("firstAttack", isFirstAttack);
                anim.SetBool("isAttacking", isAttacking);

            }
        }

        if (Time.time >= lastInputTime + inputTimer)
        {
            gotInput = false;
        }
    }
    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position, attack1Radius, whatIsDamageable);
        attackDetails.damageAmount = attack1Damage;
        attackDetails.position = transform.position;
        attackDetails.stunDamageAmount = stunDamageAmount;
        foreach (Collider2D collider in detectedObjects)
        {
            collider.transform.parent.SendMessage("Damage", attackDetails);
        }
    }
    private void CheckCombatInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (combatEnabled)
            {
                //attemptcombat
                gotInput = true;
                lastInputTime = Time.time;
            }
        }
    }

    private void FinishAttack1()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack1", false);

    }
    private void Damage(AttackDetails attackDetails)
    {
        if (!playerController.GetDashStatus())
        {

            int direction;
            playerStats.DecreaseHealth(attackDetails.damageAmount);
            if (attackDetails.position.x < transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }
            playerController.Knockback(direction);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);
    }
}
