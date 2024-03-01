using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageState : PlayerState
{
    protected bool isGrounded;
    protected bool damageDone;
    public PlayerDamageState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }


    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player.CheckIfGrounded();
    }
    public override void Enter()
    {
        base.Enter();
        damageDone = false;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        CheckKnockback();

        if (!isExitingState)
        {
            if (damageDone)
                if (isGrounded && player.CurrentVelocity.y < 0.01f)
                {
                    stateMachine.ChangeState(player.IdleState);
                }
                else
                {
                    stateMachine.ChangeState(player.InAirState);
                }
        }
    }
    private void CheckKnockback()
    {
        if (Time.time >= playerData.knockbackStartTime + playerData.knockbackDuration && playerData.knockback)
        {
            playerData.knockback = false;
            player.SetVelocityX(0f);
            player.SetVelocityY(player.RB.velocity.y);

            damageDone = true;
            player.isTakeDamage = false;

        }
    }

    public void Knockback()
    {
        playerData.knockback = true;
        playerData.knockbackStartTime = Time.time;
        player.SetVelocity(playerData.knockbackVelocity, playerData.knockbackAngle, playerData.direction);
    }
    public void Damage(AttackDetails attackDetails)
    {
        if (!player.RollState.CheckRollStatus())
        {
            DecreaseHealth(attackDetails.damageAmount);
            if (attackDetails.position.x < player.transform.position.x)
            {
                playerData.direction = 1;
            }
            else
            {
                playerData.direction = -1;
            }
            Knockback();
        }
        else
        {
            player.isTakeDamage = false;
        }
    }
    public void DecreaseHealth(float amount)
    {
        playerData.currentHealth -= amount;
        player.SetHealthBar();
        if (playerData.currentHealth <= 0.0f)
        {
            Die();
        }
    }
    private void Die()
    {
        MonoBehaviour.Instantiate(playerData.deathChunkParticle, player.transform.position, playerData.deathChunkParticle.transform.rotation);
        MonoBehaviour.Instantiate(playerData.deathBloodParticle, player.transform.position, playerData.deathBloodParticle.transform.rotation);
        MonoBehaviour.Destroy(player.gameObject);
    }



}
