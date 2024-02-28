using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToStatemachine : MonoBehaviour
{
    public AttackState attackState;
    public SpawnState spawnState;
    
    private void TriggerAttack()
    {
        attackState.TriggerAttack();
    }
    private void FinishAttack()
    {
        attackState.FinishAttack();
    }

    private void AnimationTrigger()
    {
        spawnState.AnimationTrigger();
    }
}
