using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelSystem
{
    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;


    private int currentLevel;
    private int playerExperience;
    private int experienceToNextLevel;
    private int baseExperienceToNextLevel;

    private float experienceExponentialFactor;


    public LevelSystem()
    {
        currentLevel = 1;
        playerExperience = 0;
        baseExperienceToNextLevel = 50;
        experienceToNextLevel = baseExperienceToNextLevel;
        experienceExponentialFactor = 1.5f;
    }

    public void AddExperience(int experienceAmount)
    {
        playerExperience += experienceAmount;
        CheckLevelUp();
        OnExperienceChanged?.Invoke(this, EventArgs.Empty);
    }

    private void CheckLevelUp()
    {
        while (playerExperience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }
    private void LevelUp()
    {
        currentLevel++;
        playerExperience -= experienceToNextLevel;

        experienceToNextLevel = (int)(baseExperienceToNextLevel * Mathf.Pow(2, currentLevel / experienceExponentialFactor));
        OnLevelChanged?.Invoke(this, EventArgs.Empty);
        Debug.Log("experience to next level: " + experienceToNextLevel);
    }

    public int GetCurrentLvl()
    {
        return currentLevel;
    }
    public float GetExperience()
    {
        return playerExperience;
    }
    public float GetExperienceNormalized()
    {
        return (float)playerExperience / experienceToNextLevel;
    }
}

