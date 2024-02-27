using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedState : State
{
    D_PlayerDetectedState stateData;
    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool performLongRangeAction;
    protected bool performCloseRangeAction;
    protected bool isDetectingLedge;
    public PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetectedState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }
    public override void DoChecks()
    {
        base.DoChecks();
        isDetectingLedge = entity.CheckLedge();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
    }
    public override void Enter()
    {
        base.Enter();
        performLongRangeAction = false;
        entity.SetVelocity(0f);

    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time >= startTime + stateData.longRangeActionTime)
        {
            performLongRangeAction = true;
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }
}
