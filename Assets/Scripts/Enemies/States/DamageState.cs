using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageState : State
{

    D_DamageState stateData;
    public Player player { get; private set; }


    protected bool isKnockBackOver;
    public DamageState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DamageState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    public override void DoChecks()
    {
        base.DoChecks();

    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        CheckKnockback();

    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }


    private void ActivateLifeSteal(AttackDetails attackDetalis)
    {
        if (attackDetalis.lifeStelPercentage > 0f)
        {
            float lifeStealAmount = attackDetalis.missingHealth * attackDetalis.lifeStelPercentage / 100f;
            player.RestoreHealth(lifeStealAmount);
        }
    }
    private void CheckKnockback()
    {
        if (Time.time >= stateData.knockbackStartTime + stateData.knockbackDuration && stateData.knockback)
        {
            isKnockBackOver = true;
            stateData.knockback = false;
            Debug.Log("knockback over");
            entity.SetVelocityZero();
        }
    }
    public void Knockback()
    {
        isKnockBackOver = false;
        stateData.knockback = true;
        stateData.knockbackStartTime = Time.time;
        entity.SetVelocityZero();
        entity.SetVelocity(stateData.knockbackVelocity, stateData.knockbackAngle, stateData.direction);

    }

    public virtual void Damage(AttackDetails attackDetalis)
    {

        ActivateLifeSteal(attackDetalis);
        entity.DrawParticles();
        stateData.direction = player.FacingDirection;
        Knockback();
    }

}
