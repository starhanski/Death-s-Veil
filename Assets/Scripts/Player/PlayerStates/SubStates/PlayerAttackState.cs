using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    public bool CanAttack { get; private set; }
    public bool CanMoveWhileAttack { get; private set; }

    private AttackDetails attackDetails;



    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }


    #region Animation Triggers
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        isAbilityDone = true;
        CanMoveWhileAttack = false;

    }
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        CanMoveWhileAttack = false;
        player.SetVelocityZero();
    }
    #endregion


    public override void DoChecks()
    {
        base.DoChecks();
    }
    public override void Enter()
    {
        base.Enter();
        CanAttack = false;
        CanMoveWhileAttack = true;


    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time >= startTime + playerData.attackVelocityAddingTime)
        {
            if (CanMoveWhileAttack)
            {
                player.SetVelocityX(playerData.attackVelocity * player.FacingDirection);
                player.SetVelocityY(0f);
            }
        }


    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    #region Check Functions


    public bool CheckIfCanPrimaryAttack()
    {
        return CanAttack && Time.time >= startTime + playerData.primaryAttackCooldown;
    }
    public bool CheckIfCanSecondaryAttack()
    {
        return CanAttack && Time.time >= startTime + playerData.secondaryAttackCooldown;
    }
    public override void CheckPrimaryHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(player.primaryAttackHitBox.position, playerData.primaryAttackRadius, playerData.whatIsDamageable);
        attackDetails.damageAmount = playerData.primaryAttackDamage;
        attackDetails.position = player.primaryAttackHitBox.position;
        attackDetails.stunDamageAmount = playerData.primaryAttackStunDamage;
        attackDetails.lifeStelPercentage = playerData.omnivamp;

        foreach (Collider2D collider in detectedObjects)
        {
            collider.transform.parent.SendMessage("Damage", attackDetails);
        }
    }
    public override void CheckSecondaryHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(player.secondaryAttackHitBox.position, playerData.secondaryAttackRadius, playerData.whatIsDamageable);
        attackDetails.damageAmount = playerData.secondaryAttackDamage;
        attackDetails.position = player.secondaryAttackHitBox.position;
        attackDetails.stunDamageAmount = playerData.secondaryAttackStunDamage;
        attackDetails.lifeStelPercentage = playerData.omnivamp;

        foreach (Collider2D collider in detectedObjects)
        {
            collider.transform.parent.SendMessage("Damage", attackDetails);
        }
    }
    #endregion

    #region Other Functions
    public void ResetAttack() => CanAttack = true;
    #endregion
}
