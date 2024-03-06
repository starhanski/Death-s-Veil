using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    protected D_IdleState stateData;
    protected bool flipAfterIdle;
    protected bool isIdleTimeOver;
    protected float idleTime;
    protected bool isPlayerIsMinAgroRange;
    protected bool isPlayerInAgroRadius;

    protected bool isFlipAfterAgro;
    public IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }
    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerIsMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isPlayerInAgroRadius = entity.CheckPlayerInAgroRadius();
    }
    public override void Enter()
    {
        base.Enter();
        entity.SetVelocity(0f);
        isIdleTimeOver = false;
        SetRandomIdleTime();
    }
    public override void Exit()
    {
        base.Exit();
        if (flipAfterIdle)
        {
            entity.Flip();
        }

        isFlipAfterAgro = false;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isPlayerInAgroRadius && !isPlayerIsMinAgroRange && !isFlipAfterAgro)
        {
            entity.Flip();
            isFlipAfterAgro = true;
        }
        if (Time.time >= startTime + idleTime)
        {
            isIdleTimeOver = true;
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public void SetFlipAfterIdle(bool flip)
    {
        flipAfterIdle = flip;
    }
    private void SetRandomIdleTime()
    {
        idleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime);
    }
}
