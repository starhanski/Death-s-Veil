using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
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
        player.CheckIfShouldFlip(xInput);

        if (!jumpInput)
        {
            if (isGrounded && !playerData.isOnSlope)
            {
                player.SetVelocityX(playerData.moveVelocity * xInput);
                player.SetVelocityY(0.0f);
            }
            else if (isGrounded && playerData.isOnSlope)
            {
                player.SetVelocityX(playerData.moveVelocity * playerData.slopeNormalPerpendicular.x * -xInput);
                player.SetVelocityY(playerData.moveVelocity * playerData.slopeNormalPerpendicular.y * -xInput);

            }
        }



        if (xInput == 0 && !isExitingState)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
