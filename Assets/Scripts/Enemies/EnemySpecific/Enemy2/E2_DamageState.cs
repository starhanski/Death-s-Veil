using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_DamageState : DamageState
{
    Enemy2 enemy;
    public E2_DamageState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DamageState stateData, Enemy2 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }
    public override void Enter()
    {
        base.Enter();

    }
    public override void Exit()
    {
        base.Exit();
        entity.isTakeDamage = false;
    }
    public override void DoChecks()
    {
        base.DoChecks();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (entity.isDead)
        {
            stateMachine.ChangeState(enemy.deadState);
        }
        else if (entity.isStunned && stateMachine.currentState != enemy.stunState)
        {
            stateMachine.ChangeState(enemy.stunState);
        }
        else if (isKnockBackOver)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public override void Damage(AttackDetails attackDetails)
    {
        stateMachine.ChangeState(enemy.damageState);
        base.Damage(attackDetails);

    }
}
