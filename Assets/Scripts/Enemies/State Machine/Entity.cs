
using UnityEngine;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;
    public D_Entity entityData;

    public int FacingDirection { get; private set; }
    public int LastDamageDirection { get; private set; }

    public bool IsStunned { get; private set; }
    public bool IsDead { get; private set; }

    
    public Rigidbody2D RB { get; private set; }
    public Animator Anim { get; private set; }
    public GameObject AliveGo { get; private set; }
    public AnimationToStatemachine Atsm { get; private set; }
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

    private float currentHealth, maxHealth;

    public  bool isTakeDamage;




    public virtual void Start()
    {
        SetMaxHealth();

        FacingDirection = 1;
        AliveGo = transform.Find("Alive").gameObject;
        RB = AliveGo.GetComponent<Rigidbody2D>();
        Anim = AliveGo.GetComponent<Animator>();
        stateMachine = new FiniteStateMachine();
        Atsm = AliveGo.GetComponent<AnimationToStatemachine>();
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
        velocityWorkspace.Set(FacingDirection * velocity, RB.velocity.y);
        RB.velocity = velocityWorkspace;

    }
    public virtual void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        velocityWorkspace.Set(angle.x * velocity * direction, angle.y * velocity);
        RB.velocity = velocityWorkspace;
    }
    public virtual void SetVelocityX(float velocity)
    {
        velocityWorkspace.Set(velocity, RB.velocity.y);
        RB.velocity = velocityWorkspace;
    }
    public virtual void SetVelocityY(float velocity)
    {
        velocityWorkspace.Set(RB.velocity.x, velocity);
        RB.velocity = velocityWorkspace;
    }
    public virtual void SetVelocityZero()
    {
       RB.velocity = Vector2.zero;
    }
    public virtual void SetMaxHealth(){
        maxHealth = entityData.maxHealth;
        currentHealth = maxHealth;
    }
    #endregion

    #region  Check Functions
    public virtual void CheckTouchDamage()
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
                enemyAttackDetails.position = AliveGo.transform.position;
                hit.SendMessage("Damage", enemyAttackDetails);
            }
        }
    }
    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, AliveGo.transform.right, entityData.minAgroDistance, entityData.whatIsPlayer);
    }
    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, AliveGo.transform.right, entityData.maxAgroDistance, entityData.whatIsPlayer);
    }
    public virtual bool CheckPlayerInAgroRadius(){
        return Physics2D.OverlapCircle(playerCheck.position,entityData.agroRadius,entityData.whatIsPlayer);
    }
    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, AliveGo.transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }
    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(wallCheck.position, AliveGo.transform.right, entityData.wallCheckDistance, entityData.whatIsGround);
    }
    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, entityData.ledgeCheckDistance, entityData.whatIsGround);
    }
    public virtual bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, entityData.groundCheckRadius, entityData.whatIsGround);
    }



    #endregion

    #region  Other Functions
    public virtual void Flip()
    {
        FacingDirection *= -1;
        AliveGo.transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public virtual void ResetStunResistance()
    {
        IsStunned = false;
        entityData.currentStunResistance = entityData.stunResistance;
    }
    public virtual void Damage(AttackDetails attackDetalis)
    {
        isTakeDamage = true;
        lastDamageTime = Time.time;
        currentHealth -= attackDetalis.damageAmount;
        entityData.currentStunResistance -= attackDetalis.stunDamageAmount;

        if (entityData.currentStunResistance <= 0f)
        {
            IsStunned = true;
        }

        if (currentHealth <= 0f)
        {
            IsDead = true;
        }

    }


    public virtual void DrawParticles()
    {
        Instantiate(entityData.hitParticle, AliveGo.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
    }
    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * FacingDirection * entityData.wallCheckDistance));
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData.ledgeCheckDistance));
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.closeRangeActionDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.minAgroDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.maxAgroDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position,entityData.agroRadius);



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
