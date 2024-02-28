using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityManager : MonoBehaviour
{

    [SerializeField] private PlayerData playerData;
    [SerializeField] private PlayerAbilityStats playerAbilityStats;



    [SerializeField] private Skills dashSlowTime;

    private Dictionary<string, System.Action> skillEffects;

    private Player player;
    private void Start()
    {
        player = GetComponent<Player>();
        skillEffects = new Dictionary<string, System.Action>()
        {
            { "Double Jump", ApplyDoubleJump },
            { "Roller", ApplyRoller },
            { "Dash", ApplyDash },
            { "Dash Slow Time", ApplyDashSlowTime },
            { "Jumper", ApplyJumper },
            { "Omnivamp", ApplyOmnivamp },
            { "Power", ApplyPower },
            { "Speed", ApplySpeed },
            { "Stunning", ApplyStunning },
            { "Vitality", ApplyVitality }
        };
    }

    #region Ability System Functions
    private bool CantSelectSkillMultipleTimes(Skills skill)
    {
        return skill.title == "Double Jump" || skill.title == "Roller" || skill.title == "Dash";
    }
    public void OnSkillSelected(Skills currentSkill)
    {

        if (skillEffects.ContainsKey(currentSkill.title))
        {
            skillEffects[currentSkill.title].Invoke();
            if (CantSelectSkillMultipleTimes(currentSkill))
            {
                playerAbilityStats.playerSkills.Remove(currentSkill);
            }
        }
    }

    private void ApplyDoubleJump()
    {
        playerData.amountOfJumps = 2;
    }

    private void ApplyRoller()
    {
        playerData.isCanRoll = true;
    }

    private void ApplyDash()
    {
        playerData.isCanDash = true;
        playerAbilityStats.playerSkills.Add(dashSlowTime);
    }

    private void ApplyDashSlowTime()
    {
        playerData.maxHoldTime += 0.3f;
    }

    private void ApplyJumper()
    {
        playerData.jumpVelocity *= (100f + 1f) / 100f;
        playerData.jumpHeight += 0.1f;
    }

    private void ApplyOmnivamp()
    {
        // TODO: add omnivamp mechanic
    }

    private void ApplyPower()
    {
        playerData.primaryAttackDamage *= (100f + 3f) / 100;
        playerData.secondaryAttackDamage *= (100f + 5f) / 100f;
    }

    private void ApplySpeed()
    {
        playerData.moveVelocity *= (100f + 5f) / 100f;
    }

    private void ApplyStunning()
    {
        playerData.primaryAttackStunDamage = 0.5f;
        playerData.secondaryAttackStunDamage *= (100f + 3f) / 100f;
    }

    private void ApplyVitality()
    {
        playerData.maxHealth *= (100f + 5f) / 100f;
        playerData.currentHealth = playerData.maxHealth;
        player.SetHealthBar();
    }
    #endregion
}
