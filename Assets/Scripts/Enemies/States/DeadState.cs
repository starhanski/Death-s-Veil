using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
    D_DeadState stateData;
    Player player;
    public DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DeadState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }
    public override void DoChecks()
    {
        base.DoChecks();
    }
    public override void Enter()
    {
        base.Enter();
        player = GameObject.Find("Player").GetComponent<Player>();
        GameObject.Instantiate(stateData.deathBloodParticle,entity.aliveGo.transform.position,stateData.deathBloodParticle.transform.rotation);
        GameObject.Instantiate(stateData.deathChunkParticle,entity.aliveGo.transform.position,stateData.deathChunkParticle.transform.rotation);
        entity.gameObject.SetActive(false);
        player.SendMessage("GetExperiencePoints",entity.entityData.experienceAmount);
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
