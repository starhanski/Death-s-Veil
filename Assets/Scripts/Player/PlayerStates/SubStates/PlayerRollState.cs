using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollState : PlayerAbilityState
{

    public bool CanRoll { get; private set; }

    private Vector2 rollDirection;

    private bool isRolling;

    private float lastRollTime = float.NegativeInfinity;
    public PlayerRollState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        CanRoll = false;
        player.InputHandler.UseRollInput();
        rollDirection = Vector2.right * player.FacingDirection;

    }
    public override void Exit()
    {
        base.Exit();
        isRolling = false;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!isAbilityDone)
        {
            player.SetVelocity(playerData.rollVelocity, rollDirection);
            isRolling = true;

        }
    }


    public bool CheckIfCanRoll()
    {
        return CanRoll && Time.time >= lastRollTime + playerData.rollColldown;
    }
    public bool CheckRollStatus()
    {
        return isRolling;
    }
    public void ResetCanRoll() => CanRoll = playerData.isCanRoll;
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        isAbilityDone = true;
        lastRollTime = Time.time;
        player.SetVelocityZero();
    }
}
