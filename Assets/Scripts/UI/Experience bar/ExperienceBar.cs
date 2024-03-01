using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    private Image barImage;
    private LevelSystem levelSystem;
    private void Awake()
    {
        barImage = transform.Find("expBar").GetComponent<Image>();

    }
    private void SetExperienceBarSize(float experienceNormalized)
    {
        barImage.fillAmount = experienceNormalized;
    }

    public void SetLevelSystem(LevelSystem levelSystem)
    {
        this.levelSystem = levelSystem;
        SetExperienceBarSize(levelSystem.GetExperienceNormalized());
        levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
    }

    private void LevelSystem_OnExperienceChanged(object sender, EventArgs e)
    {
        SetExperienceBarSize(levelSystem.GetExperienceNormalized());
    }

}
