using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
    public int experienceAmount;

    [Header("Checks")]
    public float wallCheckDistance = 0.1f, ledgeCheckDistance = 0.4f;
    public float minAgroDistance = 3f;
    public float maxAgroDistance = 4f;
    public float closeRangeActionDistance = 2f;
    public float groundCheckRadius = 0.3f;
    public float backAgroDistance = 4f;

    [Header("Main Stats")]
    public float maxHealth = 30f;

    public float stunResistance = 3f;
    public float stunRecoveryTime = 2f;

    [Header("Touch damage")]
    public float touchDamageWidth, touchDamageHeight;
    public float touchDamageCooldown = 1f;
    public float touchDamage = 10f;

    [Header("Other")]

    public GameObject hitParticle;
    public LayerMask whatIsPlayer;
    public LayerMask whatIsGround;
}
