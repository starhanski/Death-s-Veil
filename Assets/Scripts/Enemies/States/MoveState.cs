using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    protected D_MoveState stateData;
    protected bool isDetectingWall;
    protected bool isDetectingLedge;
    protected bool isPlayerIsMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool isPlayerInAgroRadius;

    protected bool isFlipAfterAgro;
    public MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }
    public override void DoChecks()
    {
        base.DoChecks();
        isDetectingWall = entity.CheckWall();
        isDetectingLedge = entity.CheckLedge();
        isPlayerIsMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        isPlayerInAgroRadius = entity.CheckPlayerInAgroRadius();
    }
    public override void Enter()
    {
        base.Enter();
        entity.SetVelocity(stateData.movementSpeed);
    }
    public override void Exit()
    {
        base.Exit();
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
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();


    }

}
