using UnityEngine;
using Pathfinding;
public class GhostAI : MonoBehaviour
{

    // public G_DeadState deadState { get; private set; }
    // public G_DamageState damageState { get; private set; }
    // public G_SeekState seekState { get; private set; }

    // [SerializeField]
    // private D_DeadState deadStateData;
    // [SerializeField]
    // private D_DamageState damageStateData;
    // [SerializeField]
    // private D_SeekState seekStateData;

    // public override void Start()
    // {
    //     base.Start();
    //     deadState = new G_DeadState(this, stateMachine, "dead", deadStateData, this);
    //     damageState = new G_DamageState(this, stateMachine, "damage", damageStateData, this);
    //     seekState = new G_SeekState(this, stateMachine, "seek", seekStateData, this);

    //     stateMachine.Initialize(seekState);

    // }
    // public override void Damage(AttackDetails attackDetalis)
    // {
    //     base.Damage(attackDetalis);
    //     damageState.Damage(attackDetalis);
    // }


    [SerializeField] private GameObject aliveGO;
    [SerializeField] private Transform target;
    [SerializeField] private Transform ghostTransform;
    [SerializeField] private float speed = 400f;
    [SerializeField] private float nextWaypointDistance = 3f;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;

    private Seeker seeker;
    private Rigidbody2D rb;

    private void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        aliveGO = transform.Find("Alive").gameObject;
        seeker = GetComponent<Seeker>();
        rb = aliveGO.GetComponent<Rigidbody2D>();
        ghostTransform = aliveGO.transform;
    }

    private void Start()
    {
        InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
    }

    private void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void Update()
    {
        if (path == null)
            return;

        MoveTowardsNextWaypoint();
        UpdateGhostOrientation();
    }

    private void MoveTowardsNextWaypoint()
    {
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }
        
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    private void UpdateGhostOrientation()
    {
        if (rb.velocity.x >= 0.01f)
        {
            ghostTransform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            ghostTransform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
