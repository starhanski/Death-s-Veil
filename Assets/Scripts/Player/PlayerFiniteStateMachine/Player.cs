using UnityEngine;

public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallClimbState WallClimbState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerAttackState PrimaryAttackState { get; private set; }
    public PlayerAttackState SecondaryAttackState { get; private set; }
    public PlayerRollState RollState { get; private set; }
    public PlayerDamageState DamageState { get; private set; }

    [SerializeField]
    private PlayerData playerData;
    #endregion

    #region Components
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public Animator Anim { get; private set; }
    public Transform DashDirectionIndicator { get; private set; }
    public BoxCollider2D playerBoxCollider { get; private set; }
    #endregion

    #region Check Transforms
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Transform wallCheck;
    [SerializeField]
    private Transform ledgeCheck;

    public Transform primaryAttackHitBox;
    public Transform secondaryAttackHitBox;
    #endregion

    #region Other Variables


    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }

    private Vector2 workspace;
    private Vector2 playerColliderSize;

    public GameManager GM;
    private ExperienceBar experienceBar;
    private HealthBar healthBar;

    [SerializeField]
    private SkillWindowHandler skillWindowHandler;


    private LevelSystem levelSystem = new LevelSystem();

    public bool isTakeDamage;



    #endregion

    #region Unity Callback
    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "wallClimb");
        WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "wallGrab");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir");
        LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "ledgeClimbState");
        DashState = new PlayerDashState(this, StateMachine, playerData, "dash");
        PrimaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack1");
        SecondaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack2");
        RollState = new PlayerRollState(this, StateMachine, playerData, "roll");
        DamageState = new PlayerDamageState(this, StateMachine, playerData, "damage");
    }
    private void Start()
    {
        FacingDirection = 1;
        SetMaxHealth();
        SetDefaultData();

        InputHandler = GetComponent<PlayerInputHandler>();
        Anim = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
        DashDirectionIndicator = transform.Find("DashDirectionIndicator");
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        experienceBar = GameObject.Find("UI_ExperienceBar").GetComponent<ExperienceBar>();
        healthBar = GameObject.Find("UI_HealthBar").GetComponent<HealthBar>();
        playerBoxCollider = GetComponent<BoxCollider2D>();

        playerColliderSize = playerBoxCollider.size;
        isTakeDamage = false;
        experienceBar.SetLevelSystem(levelSystem);
        skillWindowHandler.SetLevelSystem(levelSystem);

        StateMachine.Initialize(IdleState);

        SetHealthBar();
    }

    private void Update()
    {
        CurrentVelocity = RB.velocity;
        StateMachine.CurrentState.LogicUpdate();

    }
    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
        SlopeCheck();

    }
    #endregion

    #region Set Functions
    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }
    public void SetVelocity(float velocity, Vector2 direction)
    {
        workspace = direction * velocity;
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }
    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, CurrentVelocity.y);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }
    public void SetVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }
    public void SetVelocityZero()
    {
        RB.velocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;
    }


    #endregion

    #region Check Functions
    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }
    public bool CheckIfTouchingWall()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }
    public bool CheckIfTouchingWallBack()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * -FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }
    public bool CheckIfTouchingLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }
    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    public void SlopeCheck()
    {
        Vector2 checkPos = transform.position - new Vector3(0.0f, playerColliderSize.y / 2);
        SlopeCheckHorizontal(checkPos);
        SlopeCheckVeritical(checkPos);
        Debug.Log("on slope " + playerData.isOnSlope);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, playerData.slopeCheckDistance, playerData.whatIsGround);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, playerData.slopeCheckDistance, playerData.whatIsGround);

        if (slopeHitFront)
        {
            playerData.isOnSlope = true;
            playerData.slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
        }
        else if (slopeHitBack)
        {
            playerData.isOnSlope = true;
            playerData.slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);

        }
        else
        {
            playerData.slopeSideAngle = 0.0f;
            playerData.isOnSlope = false;
        }
    }
    private void SlopeCheckVeritical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, playerData.slopeCheckDistance, playerData.whatIsGround);
        if (hit)
        {
            playerData.slopeNormalPerpendicular = Vector2.Perpendicular(hit.normal).normalized;
            playerData.slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (playerData.slopeDownAngle != playerData.slopeDownAngleOld)
            {
                playerData.isOnSlope = true;
            }
            playerData.slopeDownAngleOld = playerData.slopeDownAngle;
            Debug.DrawRay(hit.point, playerData.slopeNormalPerpendicular, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }



        if (playerData.isOnSlope && InputHandler.NormInputX == 0)
        {
            RB.sharedMaterial = playerData.fullFriction;
        }
        else
        {
            RB.sharedMaterial = playerData.noFriction;
        }
    }
    #endregion

    #region Other Functions
    public Vector2 DetermineCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
        float xDist = xHit.distance;
        workspace.Set(xDist * FacingDirection, 0f);
        RaycastHit2D yHit = Physics2D.Raycast(ledgeCheck.position + (Vector3)(workspace), Vector2.down, ledgeCheck.position.y - wallCheck.position.y, playerData.whatIsGround);
        float yDist = yHit.distance;

        workspace.Set(wallCheck.position.x + (xDist * FacingDirection), ledgeCheck.position.y - yDist);
        return workspace;
    }
    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    private void CheckPrimaryHitBox() => StateMachine.CurrentState.CheckPrimaryHitBox();
    private void CheckSecondaryHitBox() => StateMachine.CurrentState.CheckSecondaryHitBox();
    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    private void Damage(AttackDetails attackDetails)
    {
        isTakeDamage = true;
        DamageState.Damage(attackDetails);
    }
    public void SetMaxHealth() => playerData.currentHealth = playerData.maxHealth;
    public void SetHealthBar()
    {
        healthBar.SetHealthBar(playerData.currentHealth, playerData.maxHealth);
    }
    public void SetDefaultData()
    {
        playerData.isCanDash = false;
        playerData.isCanRoll = false;
        playerData.amountOfJumps = 1;
        playerData.maxHealth = 50f;
        playerData.moveVelocity = 10f;
        playerData.jumpVelocity = 25f;
        playerData.maxHoldTime = 0.5f;
        playerData.primaryAttackDamage = 5f;
        playerData.secondaryAttackDamage = 10f;
        playerData.primaryAttackStunDamage = 0f;
        playerData.secondaryAttackStunDamage = 1f;
        playerData.omnivamp = 0;

    }
    #endregion

    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(primaryAttackHitBox.position, playerData.primaryAttackRadius);
        Gizmos.DrawWireSphere(secondaryAttackHitBox.position, playerData.secondaryAttackRadius);
    }
    #endregion

    #region Level System


    public void GetExperiencePoints(int experienceAmount)
    {
        levelSystem.AddExperience(experienceAmount);
    }


    #endregion


}
