// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Pathfinding;

// public class SeekState : State
// {
//     D_SeekState stateData;

//     [SerializeField] private GameObject aliveGO;
//     [SerializeField] private Transform ghostTransform;
//     [SerializeField] private float speed = 400f;
//     [SerializeField] private float nextWaypointDistance = 3f;



//     private Path path;
//     private int currentWaypoint = 0;
//     private bool reachedEndOfPath = false;

//     private Seeker seeker;
//     private Rigidbody2D rb;

//     public SeekState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_SeekState stateData) : base(entity, stateMachine, animBoolName)
//     {
//         this.stateData = stateData;
//         InitializeComponents();
//     }
//     public override void DoChecks()
//     {
//         base.DoChecks();
//     }
//     public override void Enter()
//     {
//         base.Enter();
//         entity.InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
//     }
//     public override void Exit()
//     {
//         base.Exit();
//     }
//     public override void LogicUpdate()
//     {
//         base.LogicUpdate();

//         if (path == null)
//         {
//             return;
//         }

//         MoveTowardsNextWaypoint();
//         UpdateGhostOrientation();
//     }
//     public override void PhysicsUpdate()
//     {
//         base.PhysicsUpdate();
//     }


//     private void InitializeComponents()
//     {
//         aliveGO = entity.AliveGo;
//         seeker = entity.GetComponent<Seeker>();
//         rb = aliveGO.GetComponent<Rigidbody2D>();
//         ghostTransform = aliveGO.transform;

//     }
//     protected void UpdatePath()
//     {
//         if (seeker.IsDone())
//         {
//             seeker.StartPath(rb.position, stateData.target.position, OnPathComplete);
//         }
//     }

//     private void OnPathComplete(Path p)
//     {
//         if (!p.error)
//         {
//             path = p;
//             currentWaypoint = 0;
//         }
//     }
//     private void MoveTowardsNextWaypoint()
//     {
//         if (currentWaypoint >= path.vectorPath.Count)
//         {
//             reachedEndOfPath = true;
//             return;
//         }
//         else
//         {
//             reachedEndOfPath = false;
//         }

//         Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
//         Vector2 force = direction * speed * Time.deltaTime;

//         rb.AddForce(force);

//         float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

//         if (distance < nextWaypointDistance)
//         {
//             currentWaypoint++;
//         }
//     }

//     private void UpdateGhostOrientation()
//     {
//         if (rb.velocity.x >= 0.01f)
//         {
//             ghostTransform.localScale = new Vector3(-1f, 1f, 1f);
//         }
//         else if (rb.velocity.x <= -0.01f)
//         {
//             ghostTransform.localScale = new Vector3(1f, 1f, 1f);
//         }
//     }
// }
