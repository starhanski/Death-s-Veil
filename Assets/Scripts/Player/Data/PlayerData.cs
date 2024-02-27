using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Main Stats")]
    public float maxHealth = 50;
    public float currentHealth;
    public float experience;
    public int level = 1;

    [Header("Move State")]
    public float moveVelocity = 10f;

    [Header("Jump State")]
    public float jumpVelocity = 15f;
    public int amountOfJumps = 1;

    [Header("In Air State")]
    public float coyoteTime = 0.2f;
    public float jumpHeight = 0.5f;

    [Header("Wall Slide State")]
    public float wallSlideVelocity = 3f;

    [Header("Wall Climb State")]
    public float wallClimbVelocity = 3f;

    [Header("Wall Jump State")]
    public float wallJumpVelocity = 20f;
    public float wallJumpTime = 0.4f;
    public Vector2 wallJumpAngle = new Vector2(1, 2);

    [Header("Ledge Climb State")]
    public Vector2 startOffset;
    public Vector2 stopOffset;

    [Header("Dash State")]
    public bool isCanDash = false;
    public float dashCooldown = 1f;
    public float maxHoldTime = 1f;
    public float holdTimeScale = 0.25f;
    public float dashTime = 0.25f;
    public float dashVelocity = 30f;
    public float drag = 10f;
    public float dashEndYMultiplier = 0.2f;
    public float distanceBetweenAfterImages;

    [Header("Roll State")]
    public bool isCanRoll = false;
    public float rollVelocity = 10f;
    public float rollColldown = 5f;

    [Header("Attack State")]
    public float primaryAttackDamage = 10f;
    public float secondaryAttackDamage = 10f;
    public float primaryAttackRadius = 5f;
    public float secondaryAttackRadius = 8f;

    public float primaryAttackCooldown = 0.4f;
    public float secondaryAttackCooldown = 1f;
    public float attackVelocity = 5f;
    public float attackVelocityAddingTime = 0.5f;
    public float secondaryAttackStunDamage = 1f;
    public float primaryAttackStunDamage = 1f;

    public float omnivamp = 0f;
    public LayerMask whatIsDamageable;



    [Header("Knockback")]
    public int direction;
    public bool knockback;

    public float knockbackVelocity = 10f;
    public float knockbackStartTime;
    public float knockbackDuration = 1f;

    public Vector2 knockbackAngle = new Vector2(1, 2);

    [Header("Damage State")]

    public GameObject deathChunkParticle, deathBloodParticle;

    [Header("Check Variables")]
    public float wallCheckDistance = 0.5f;
    public float groundCheckRadius = 0.3f;
    public LayerMask whatIsGround;

    [Header("Slopes Variables")]

    public float slopeCheckDistance = 0.5f;
    public float slopeDownAngle;
    public float slopeDownAngleOld;


    public float slopeSideAngle;
    public float maxSlopeAngle;
    public Vector2 slopeNormalPerpendicular;

    public bool isOnSlope;

    public PhysicsMaterial2D noFriction;
    public PhysicsMaterial2D fullFriction;

}
