using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAbilityData", menuName = "Data/Player Data/Ability Data")]
public class PlayerAbilityStats : ScriptableObject
{

   

    [Header("All Player Skills")]
    public List<Skills> playerSkills = new List<Skills>();


}
