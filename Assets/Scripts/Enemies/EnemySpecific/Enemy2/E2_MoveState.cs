using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_MoveState : MoveState
{
    Enemy2 enemy;
    public E2_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, Enemy2 enemy) : base(entity, stateMachine, animBoolName, stateData)
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
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
       
        if (isDetectingWall || !isDetectingLedge)
        {
            entity.SetVelocity(0f);
            entity.Flip();
            entity.SetVelocity(stateData.movementSpeed);

        }
        if (isPlayerIsMinAgroRange)
        {
            stateMachine.ChangeState(enemy.playerDetectedState);
        }

    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
