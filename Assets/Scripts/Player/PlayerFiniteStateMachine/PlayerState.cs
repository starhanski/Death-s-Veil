using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected float startTime;

    private string animBoolName;

    protected bool isAnimationFinished;
    protected bool isExitingState;
    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        startTime = Time.time;
        DoChecks();
        player.Anim.SetBool(animBoolName, true);
        isAnimationFinished = false;
        isExitingState = false;

    }
    public virtual void Exit()
    {
        player.Anim.SetBool(animBoolName, false);
        isExitingState = true;

    }
    public virtual void LogicUpdate()
    {

    }
    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }
    public virtual void DoChecks() { }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;

    public virtual void CheckPrimaryHitBox(){}
    public virtual void CheckSecondaryHitBox(){}


}
