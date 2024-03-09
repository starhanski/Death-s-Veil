using UnityEngine;
using Pathfinding;
public class GhostAI : Entity
{

    public G_DeadState deadState { get; private set; }
    public G_DamageState damageState { get; private set; }
    public G_SeekState seekState { get; private set; }

    [SerializeField]
    private D_DeadState deadStateData;
    [SerializeField]
    private D_DamageState damageStateData;
    [SerializeField]
    private D_SeekState seekStateData;

    public override void Start()
    {
        base.Start();
        deadState = new G_DeadState(this, stateMachine, "dead", deadStateData, this);
        damageState = new G_DamageState(this, stateMachine, "damage", damageStateData, this);
        seekState = new G_SeekState(this, stateMachine, "seek", seekStateData, this);

        stateMachine.Initialize(seekState);
        InvokeRepeating(nameof(InvokeSeekStateUpdatePath), 0f, 0.5f);

    }
    private void InvokeSeekStateUpdatePath()
    {
        seekState.UpdatePath();
    }
    public override void Damage(AttackDetails attackDetalis)
    {
        base.Damage(attackDetalis);
        damageState.Damage(attackDetalis);
    }



}
