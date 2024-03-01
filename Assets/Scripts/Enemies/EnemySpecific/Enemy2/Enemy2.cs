using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Entity
{

    #region States Variables
    public E2_MoveState moveState { get; private set; }
    public E2_LookForPlayerState lookForPlayerState { get; private set; }
    public E2_DeadState deadState { get; private set; }
    public E2_PlayerDetectedState playerDetectedState { get; private set; }
    public E2_StunState stunState { get; private set; }
    public E2_ChargeState chargeState { get; private set; }

    #endregion

    #region Data
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_LookForPlayerState lookForPlayerStateData;
    [SerializeField]
    private D_StunState stunStateData;
    [SerializeField]
    private D_DeadState deadStateData;
    [SerializeField]
    private D_PlayerDetectedState playerDetectedStateData;
    [SerializeField]
    private D_ChargeState chargeStateData;
    #endregion


    public override void Start()
    {
        base.Start();
        moveState = new E2_MoveState(this, stateMachine, "move", moveStateData, this);
        lookForPlayerState = new E2_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        deadState = new E2_DeadState(this, stateMachine, "dead", deadStateData, this);
        playerDetectedState = new E2_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedStateData, this);
        stunState = new E2_StunState(this, stateMachine, "stun", stunStateData, this);
        chargeState = new E2_ChargeState(this, stateMachine, "charge", chargeStateData, this);

        stateMachine.Initialize(moveState);
    }

    public override void Damage(AttackDetails attackDetalis)
    {
        base.Damage(attackDetalis);
        
        if (isDead)
        {
            stateMachine.ChangeState(deadState);
        }
        else if (isStunned && stateMachine.currentState != stunState)
        {
            stateMachine.ChangeState(stunState);
        }

    }
}
