using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDamageStateData", menuName = "Data/State Data/Damage State")]

public class D_DamageState : ScriptableObject
{
    [Header("Knockback")]
    public int direction;
    public bool knockback;

    public float knockbackVelocity = 10f;
    public float knockbackStartTime;
    public float knockbackDuration = 1f;
    public Vector2 knockbackAngle = new Vector2(1, 2);


 
}
