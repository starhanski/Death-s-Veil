using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrabState : PlayerTouchingWallState
{
    private Vector2 holdPosition;
    public PlayerWallGrabState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }
    public override void DoChecks()
    {
        base.DoChecks();
    }
    public override void Enter()
    {
        base.Enter();
        holdPosition = player.transform.position;
        HoldPostion();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!isExitingState)
        {
            HoldPostion();
            if (yInput > 0)
            {
                stateMachine.ChangeState(player.WallClimbState);
            }
            else if (yInput < 0 || !grabInput)
            {
                stateMachine.ChangeState(player.WallSlideState);
            }
        }

    }
    private void HoldPostion()
    {
        player.transform.position = holdPosition;
        player.SetVelocityX(0f);
        player.SetVelocityY(0f);
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
