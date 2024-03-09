using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;

public class SeekState : State
{
    D_SeekState stateData;

    [SerializeField] private GameObject aliveGO;
    [SerializeField] private Transform ghostTransform;
    


    private Transform target;
    private Path path;
    private int currentWaypoint = 0;

    private Seeker seeker;
    private Rigidbody2D rb;

    public SeekState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_SeekState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
        InitializeComponents();
    }



    public override void Enter()
    {
        InitializeComponents();

    }

    private void InitializeComponents()
    {
        aliveGO = entity.AliveGo;
        seeker = entity.GetComponent<Seeker>();
        rb = aliveGO.GetComponent<Rigidbody2D>();
        ghostTransform = aliveGO.transform;
        target = GameObject.Find("Player").gameObject.GetComponent<Transform>();
    }

    

    public  void UpdatePath()
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

    public override void LogicUpdate()
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
            return; 
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * stateData.speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < stateData.nextWaypointDistance)
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
