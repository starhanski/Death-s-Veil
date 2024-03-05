
using UnityEngine;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;
    public D_Entity entityData;

    public int facingDirection { get; private set; }
    public int lastDamageDirection { get; private set; }

    public bool isStunned { get; private set; }
    public bool isDead { get; private set; }
    public bool isPlayerOnRightSide{get; private set;}

    
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public GameObject aliveGo { get; private set; }
    public AnimationToStatemachine atsm { get; private set; }
    public AttackDetails enemyAttackDetails;



    [SerializeField]
    private Transform wallCheck;
    [SerializeField]
    private Transform ledgeCheck;
    [SerializeField]
    private Transform playerCheck;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Transform touchDamageCheck;

    private Vector2 velocityWorkspace;
    private Vector2 touchDamageBotLeft, touchDamageTopRight;


    private float lastTouchDamageTime;
    private float lastDamageTime;

    public  bool isTakeDamage;




    public virtual void Start()
    {
        SetMaxHealth();

        facingDirection = 1;
        aliveGo = transform.Find("Alive").gameObject;
        rb = aliveGo.GetComponent<Rigidbody2D>();
        anim = aliveGo.GetComponent<Animator>();
        stateMachine = new FiniteStateMachine();
        atsm = aliveGo.GetComponent<AnimationToStatemachine>();
    }

    public virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();
        if (Time.time >= lastDamageTime + entityData.stunRecoveryTime)
        {
            ResetStunResistance();
        }

        CheckTouchDamage();
    }
    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    #region Set Functions
    public virtual void SetVelocity(float velocity)
    {
        velocityWorkspace.Set(facingDirection * velocity, rb.velocity.y);
        rb.velocity = velocityWorkspace;

    }
    public virtual void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        velocityWorkspace.Set(angle.x * velocity * direction, angle.y * velocity);
        rb.velocity = velocityWorkspace;
    }
    public virtual void SetVelocityX(float velocity)
    {
        velocityWorkspace.Set(velocity, rb.velocity.y);
        rb.velocity = velocityWorkspace;
    }
    public virtual void SetVelocityY(float velocity)
    {
        velocityWorkspace.Set(rb.velocity.x, velocity);
        rb.velocity = velocityWorkspace;
    }
    public virtual void SetVelocityZero()
    {
       rb.velocity = Vector2.zero;
    }
    public virtual void SetMaxHealth(){
        entityData.currentHealth = entityData.maxHealth;
    }
    #endregion

    #region  Check Functions
    private void CheckTouchDamage()
    {
        if (Time.time >= lastTouchDamageTime + entityData.touchDamageCooldown)
        {
            touchDamageBotLeft.Set(touchDamageCheck.position.x - (entityData.touchDamageWidth / 2), touchDamageCheck.position.y - (entityData.touchDamageHeight / 2));
            touchDamageTopRight.Set(touchDamageCheck.position.x + (entityData.touchDamageWidth / 2), touchDamageCheck.position.y + (entityData.touchDamageHeight / 2));
            Collider2D hit = Physics2D.OverlapArea(touchDamageBotLeft, touchDamageTopRight, entityData.whatIsPlayer);
            if (hit != null)
            {
                lastTouchDamageTime = Time.time;
                enemyAttackDetails.damageAmount = entityData.touchDamage;
                enemyAttackDetails.position = aliveGo.transform.position;
                hit.SendMessage("Damage", enemyAttackDetails);
            }
        }
    }
    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, aliveGo.transform.right, entityData.minAgroDistance, entityData.whatIsPlayer);
    }
    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, aliveGo.transform.right, entityData.maxAgroDistance, entityData.whatIsPlayer);
    }
    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, aliveGo.transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }
    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(wallCheck.position, aliveGo.transform.right, entityData.wallCheckDistance, entityData.whatIsGround);
    }
    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, entityData.ledgeCheckDistance, entityData.whatIsGround);
    }
    public virtual bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, entityData.groundCheckRadius, entityData.whatIsGround);
    }

    public virtual void CheckPlayerSide(AttackDetails attackDetalis)
    {
       if(attackDetalis.position.x >= aliveGo.transform.position.x)
       {
        isPlayerOnRightSide = true;
       }
       else 
       {
        isPlayerOnRightSide = false;
       }
    }

    #endregion

    #region  Other Functions
    public virtual void Flip()
    {
        facingDirection *= -1;
        aliveGo.transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public virtual void ResetStunResistance()
    {
        isStunned = false;
        entityData.currentStunResistance = entityData.stunResistance;
    }
    public virtual void Damage(AttackDetails attackDetalis)
    {
        CheckPlayerSide(attackDetalis);
        isTakeDamage = true;
        lastDamageTime = Time.time;
        entityData.currentHealth -= attackDetalis.damageAmount;
        entityData.currentStunResistance -= attackDetalis.stunDamageAmount;

        if (entityData.currentStunResistance <= 0f)
        {
            isStunned = true;
        }

        if (entityData.currentHealth <= 0f)
        {
            isDead = true;
        }

    }


    public virtual void DrawParticles()
    {
        Instantiate(entityData.hitParticle, aliveGo.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
    }
    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.wallCheckDistance));
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData.ledgeCheckDistance));
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.closeRangeActionDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.minAgroDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.maxAgroDistance), 0.2f);



        Vector2
       bottomLeft = new Vector2(touchDamageCheck.position.x - (entityData.touchDamageWidth / 2), touchDamageCheck.position.y - (entityData.touchDamageHeight / 2)),
       bottomRight = new Vector2(touchDamageCheck.position.x + (entityData.touchDamageWidth / 2), touchDamageCheck.position.y - (entityData.touchDamageHeight / 2)),
       topRight = new Vector2(touchDamageCheck.position.x + (entityData.touchDamageWidth / 2), touchDamageCheck.position.y + (entityData.touchDamageHeight / 2)),
       topLeft = new Vector2(touchDamageCheck.position.x - (entityData.touchDamageWidth / 2), touchDamageCheck.position.y + (entityData.touchDamageHeight / 2));
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);

    }
    #endregion
}
