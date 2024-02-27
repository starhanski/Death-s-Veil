using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private Vector2 detectedPosition;
    private Vector2 cornerPostion;
    private Vector2 startPosition;
    private Vector2 stopPosition;

    private bool isHanging;
    private bool isClimb;
    private bool jumpInput;

    private int xInput;
    private int yInput;
    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public void SetDetectedPosition(Vector2 position) => detectedPosition = position;
    public override void Enter()
    {
        base.Enter();
        player.SetVelocityZero();
        player.transform.position = detectedPosition;
        cornerPostion = player.DetermineCornerPosition();

        startPosition.Set(cornerPostion.x - (player.FacingDirection * playerData.startOffset.x), cornerPostion.y - playerData.startOffset.y);
        stopPosition.Set(cornerPostion.x + (player.FacingDirection * playerData.stopOffset.x), cornerPostion.y + playerData.stopOffset.y);

        player.transform.position = startPosition;
    }
    public override void Exit()
    {
        base.Exit();
        isHanging = false;
        if (isClimb)
        {
            player.transform.position = stopPosition;
            isClimb = false;
        }
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isAnimationFinished)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else
        {
            xInput = player.InputHandler.NormInputX;
            yInput = player.InputHandler.NormInputY;
            jumpInput = player.InputHandler.JumpInput;

            player.SetVelocityZero();
            player.transform.position = startPosition;

            if (xInput == player.FacingDirection && isHanging && !isClimb)
            {
                isClimb = true;
                player.Anim.SetBool("climbLedge", true);
            }
            else if (yInput ==-1 && isHanging && !isClimb)
            {
                stateMachine.ChangeState(player.InAirState);
            }
            else if(jumpInput && !isClimb)
            {
                player.WallJumpState.DetermineWallJumpDirection(true);
                stateMachine.ChangeState(player.WallJumpState);
            }
        }
    }
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        isHanging = true;
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        player.Anim.SetBool("climbLedge", false);

    }
}
