using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnState : State
{
    D_SpawnState stateData;
    protected bool isAnimationFinished;
    public SpawnState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_SpawnState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }
    public override void Enter()
    {
        entity.Atsm.spawnState = this;
        isAnimationFinished = false;
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

    public virtual void AnimationTrigger()
    {
        isAnimationFinished = true;
    }

}
