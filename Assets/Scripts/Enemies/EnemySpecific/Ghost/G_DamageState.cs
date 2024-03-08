using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_DamageState : DamageState
{
    GhostAI enemy;

    public G_DamageState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DamageState stateData, GhostAI enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }
    public override void Damage(AttackDetails attackDetalis)
    {
        base.Damage(attackDetalis);
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
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
