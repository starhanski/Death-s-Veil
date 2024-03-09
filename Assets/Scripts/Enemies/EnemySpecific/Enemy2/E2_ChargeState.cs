using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_ChargeState : ChargeState
{
    Enemy2 enemy;
    public E2_ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData, Enemy2 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
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
       
        if (!isDetectingLedge || isDetectingWall)
        {
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }
        else if (isChargeTimeOver)
        {
            if (isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(enemy.playerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }

        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
