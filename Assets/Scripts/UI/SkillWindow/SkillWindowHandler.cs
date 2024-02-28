using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillWindowHandler : MonoBehaviour
{
    private bool seeSkillWindow;
    private LevelSystem levelSystem;
    private Animator animationController;


    [SerializeField]
    private PlayerAbilityStats playerAbilityStats;

    [SerializeField]
    private List<SkillSection> skillSections = new List<SkillSection>();

    private void Awake()
    {
        animationController = GetComponent<Animator>();
        playerAbilityStats.playerSkills = new List<Skills>(playerAbilityStats.startedPlayerSkills);
    }

    public void SetSkillWindow()
    {
        seeSkillWindow = !seeSkillWindow;
        gameObject.SetActive(seeSkillWindow);

        if (seeSkillWindow)
        {
            animationController.SetBool("show", true);

        }
        
    }

    public void SetLevelSystem(LevelSystem levelSystem)
    {
        this.levelSystem = levelSystem;
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
    {
        SetSkillWindow();
        FillSkillWindowWithAbilities();

    }

    private void FillSkillWindowWithAbilities()
    {
        List<Skills> availableSkills = new List<Skills>(playerAbilityStats.playerSkills);
        for (int i = 0; i < skillSections.Count; i++)
        {
            Skills randomSkill = GetRandomSkill(availableSkills);
            if (randomSkill != null)
            {
                SetValues(skillSections[i], randomSkill);
                availableSkills.Remove(randomSkill);
            }
        }
    }

    private void SetValues(SkillSection section, Skills skill)
    {
        section.icon.sprite = skill.icon;
        section.title.text = skill.title;
        section.description.text = skill.description;
        section.currentSkill = skill;
    }

    private Skills GetRandomSkill(List<Skills> availableSkills)
    {
        if (availableSkills.Count == 0)
        {
            return null;
        }
        int randomIndex = UnityEngine.Random.Range(0, availableSkills.Count);
        return availableSkills[randomIndex];
    }



}
